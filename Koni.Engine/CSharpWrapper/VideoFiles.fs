// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine.Wrapper

open System.Collections.ObjectModel
open System.ComponentModel
open System.IO.Abstractions
open Koni.Engine

type VideoFile(path, presets: Presets, filesystem) =
    let _fs = filesystem
    let _presets = presets.Items |> Seq.map (fun p -> Preset.create p.SearchFor p.ReplaceWith)
    let mutable _isSaved = false
    let mutable _item = VideoFile.create path _presets _fs

    let event = new Event<_,_>()
    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = event.Publish

    member this.Path = _item.FilePath
    member this.FileName = _item.FileName
    member this.Title 
        with get() = _item.Title
        and set(value) = 
            _item <- VideoFile.update _item value
            event.Trigger(this, PropertyChangedEventArgs("Title"))
    member this.IsSaved
        with get() = _isSaved
        and set(value) = _isSaved <- value
    member this.Reset() = 
        _item <- VideoFile.reset _item _presets _fs
        event.Trigger(this, PropertyChangedEventArgs("Title"))
    member this.Save() = 
        VideoFile.save _item
        _isSaved <- true
        event.Trigger(this, PropertyChangedEventArgs("IsSaved"))

type VideoFiles(presets: Presets, filesystem) =
    let _fs = filesystem
    let mutable _presets = presets
    let _presets' = presets.Items |> Seq.map (fun p -> Preset.create p.SearchFor p.ReplaceWith)
    let mutable _items = new ObservableCollection<VideoFile>()
    member this.Presets = _presets
    member this.Items = _items
    member this.Add(items: string seq) = 
        VideoFiles.create items _presets' _fs
        |> Seq.map (fun i -> new VideoFile(i.FilePath, _presets, _fs))
        |> Seq.iter (fun i -> _items.Add(i))
    member this.UpdatePresets(presets) = _presets <- presets
    member this.ResetItems() =
        let items = _items |> Seq.map (fun v -> v.Path) |> Seq.toList
        this.ClearAll()
        this.Add(items)
    member this.Rename(index, title) =
        if index <> -1 then
            _items.[index].Title <- title
    member this.Delete(index) =
        if index <> -1 then
            _items.RemoveAt(index)
    member this.ClearAll() =
        let items = _items |> Seq.toList
        items |> Seq.iter (fun i -> _items.Remove(i) |> ignore)

    new(presets) = VideoFiles(presets, new FileSystem())