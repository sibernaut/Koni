// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine.Wrapper

open System.Collections.ObjectModel
open Koni.Engine

type Preset(search, replace) = 
    let _item = 
        { SearchFor = search
          ReplaceWith = replace }
    member this.SearchFor
        with get() = _item.SearchFor
        and set(value) = _item.SearchFor <- value
    member this.ReplaceWith
        with get() = _item.ReplaceWith
        and set(value) = _item.ReplaceWith <- value
    static member Default = 
        let item = Preset.defaultPreset
        new Preset(item.SearchFor, item.ReplaceWith)

type Presets() =
    let _items = new ObservableCollection<Preset>()
    member this.Items = _items
    member this.Apply(input) = 
        let castItems = 
            this.Items
            |> Seq.map (fun i -> 
                { SearchFor = i.SearchFor
                  ReplaceWith = i.ReplaceWith })
        Presets.apply castItems input
    member this.Add(search, replace) = 
        let item = Preset(search, replace)
        this.Items.Add(item)
    member this.Update(index, search, replace) =
        this.Items.[index] <- Preset(search, replace)
    member this.Delete(index) =
        this.Items.RemoveAt(index)
    member this.MoveUp(index) =
        if not (index = 0) then
            this.Items.Move(index, index - 1)
    member this.MoveDown(index) =
        if not (index = this.Items.Count - 1) then
            this.Items.Move(index, index + 1)
