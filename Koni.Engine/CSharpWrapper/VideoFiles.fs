// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine.Wrapper

open System.Collections.ObjectModel
open System.ComponentModel
open System.IO.Abstractions
open Koni.Engine

type VideoFile(item) =
    let mutable _isSaved = false
    let mutable _item = item

    let event = new Event<_,_>()
    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = event.Publish

    member this.Path = _item.FilePath
    member this.FileName = _item.FileName
    member this.Title 
        with get() = _item.Title
        and set(value) = 
            _item <- VideoFile.updateTitle _item value
            event.Trigger(this, PropertyChangedEventArgs("Title"))
    member this.IsSaved
        with get() = _isSaved
        and set(value) = _isSaved <- value
    member this.UpdateConfig(config: Config) = _item <- VideoFile.updateConfig _item config.Item
    member this.Reset() = 
        _item <- VideoFile.resetTitle _item
        event.Trigger(this, PropertyChangedEventArgs("Title"))
    member this.Save() = 
        VideoFile.save _item
        _isSaved <- true
        event.Trigger(this, PropertyChangedEventArgs("IsSaved"))

type VideoFiles(config: Config, filesystem) =
    let _items = new ObservableCollection<VideoFile>()
    member this.Items = _items
    member this.Add(items: string seq) = 
        VideoFile.Collection.create items config.Item filesystem
        |> Seq.map (fun v -> new VideoFile(v))
        |> Seq.iter (fun v -> _items.Add(v))
    member this.UpdateConfig(config) = _items |> Seq.iter (fun v -> v.UpdateConfig(config))
    member this.ResetItems() = _items |> Seq.iter (fun v -> v.Reset())
    member this.Rename(index, title) =
        if index <> -1 then
            _items.[index].Title <- title
    member this.Delete(index) =
        if index <> -1 then
            _items.RemoveAt(index)
    member this.ClearAll() =
        let items = _items |> Seq.toList
        items |> Seq.iter (fun i -> _items.Remove(i) |> ignore)

    new(config) = VideoFiles(config, new FileSystem())