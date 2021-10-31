namespace Koni.Engine.Wrapper

open System.Collections.ObjectModel
open System.ComponentModel
open Koni.Engine

type VideoFile(path, presets: Presets, filesystem) =
    let mutable _isSaved = false
    let castPresets =
        presets.Items 
        |> Seq.map (fun p -> 
            { SearchFor = p.SearchFor
              ReplaceWith = p.ReplaceWith })
    let _item = Berkas.create path castPresets filesystem
    let ev = new Event<_,_>()
    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = ev.Publish
    member this.Path = _item.FilePath
    member this.FileName = _item.FileName
    member this.Title = _item.Title
    member this.IsSaved
        with get() = _isSaved
        and set(value) = 
            _isSaved <- value
            ev.Trigger(this, PropertyChangedEventArgs("IsSaved"))
    member this.Save() = 
        Berkas.save _item
        this.IsSaved <- true

type VideoFiles(presets: Presets, filesystem) =
    let mutable _presets = presets
    let mutable _items = new ObservableCollection<VideoFile>()
    let ev = new Event<_,_>()
    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = ev.Publish
    member this.FileSystem = filesystem
    member this.Presets
        with get() = _presets
        and set(value) = _presets <- value
    member this.Items
        with get() = _items
        and set(value) = 
            _items <- value
            ev.Trigger(this, PropertyChangedEventArgs("Items"))
    member this.Add(items: string[]) = 
        let castPresets =
            this.Presets.Items
            |> Seq.map (fun p -> 
                { SearchFor = p.SearchFor
                  ReplaceWith = p.ReplaceWith })
        BerkasBerkas.create items castPresets this.FileSystem
        |> Seq.map (fun i -> new VideoFile(i.FilePath, this.Presets, this.FileSystem))
        |> Seq.iter (fun i -> _items.Add(i))
    member this.Update() =
        let items = new ObservableCollection<VideoFile>()
        _items
        |> Seq.map (fun i -> new VideoFile(i.Path, this.Presets, this.FileSystem))
        |> Seq.iter (fun i -> items.Add(i))
        this.Items <- items
