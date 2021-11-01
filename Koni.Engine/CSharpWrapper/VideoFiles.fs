// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

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
    let mutable _title = _item.Title
    let ev = new Event<_,_>()
    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = ev.Publish
    member this.Path = _item.FilePath
    member this.FileName = _item.FileName
    member this.Title 
        with get() = _title
        and set(value) = 
            _title <- value
            _item.Title <- value
            ev.Trigger(this, PropertyChangedEventArgs("Title"))
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
        |> Seq.iter (fun i -> this.Items.Add(i))
    member this.Update() =
        let items = this.Items |> Seq.toList
        this.ClearAll()
        items |> Seq.iter (fun i -> this.Items.Add(i))
    member this.Rename(index, title) =
        if index <> -1 then
            this.Items.[index].Title <- title
    member this.Delete(index) =
        if index <> -1 then
            this.Items.RemoveAt(index)
    member this.ClearAll() =
        let items = this.Items |> Seq.toList
        items |> Seq.iter (fun i -> this.Items.Remove(i) |> ignore)