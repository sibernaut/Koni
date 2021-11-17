// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine

open System.Collections.ObjectModel
open System.IO.Abstractions
open Koni.Engine

type VideoFiles(config: Config, filesystem) =
    let _items = new ObservableCollection<Video>()
    member val Config = config with get, set
    member this.Items = _items
    member this.FileSystem = filesystem
    member this.Add(items: string seq) = 
        Video.Collection.create items config.Item filesystem
        |> Seq.iter (fun v -> _items.Add(v))
    member this.UpdateConfig(config) = this.Config <- config
    member this.ResetItem(index) = 
        _items.[index] <- Video.resetTitle _items.[index] this.Config.Item this.FileSystem
    member this.ResetItems() = 
        for i in 0.._items.Count - 1 do
            _items.[i] <- Video.resetTitle _items.[i] this.Config.Item this.FileSystem
    member this.Rename(index, title) =
        if index <> -1 then
            _items.[index] <- Video.updateTitle _items.[index] title
    member this.Remove(item) = _items.Remove(item)
    member this.ClearAll() = _items.Clear()
    member this.Save() = 
        for i in 0.._items.Count - 1 do
            _items.[i] <- Video.save _items.[i] this.FileSystem

    new(config) = VideoFiles(config, new FileSystem())