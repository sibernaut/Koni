// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine.Wrapper

open System.Collections.ObjectModel
open Koni.Engine

type Preset(search, replace) = 
    let mutable _item = Preset.create search replace
    member this.SearchFor
        with get() = _item.SearchFor
        and set(value) = _item <- Preset.updateSearch _item value
    member this.ReplaceWith
        with get() = _item.ReplaceWith
        and set(value) = _item <- Preset.updateReplace _item value
    static member Default = 
        let item = Preset.defaultPreset
        new Preset(item.SearchFor, item.ReplaceWith)

type Presets() =
    let mutable _items = new ObservableCollection<Preset>()    
    let _items' = _items |> Seq.map (fun i -> Preset.create i.SearchFor i.ReplaceWith)
    member this.Items = _items
    member this.Apply(input) = Presets.apply _items' input
    member this.Add(search, replace) = _items.Add(Preset(search, replace))
    member this.Update(index, search, replace) = _items.[index] <- Preset(search, replace)
    member this.Delete(index) = _items.RemoveAt(index)
    member this.MoveUp(index) = if not (index = 0) then _items.Move(index, index - 1)
    member this.MoveDown(index) = if not (index = _items.Count - 1) then _items.Move(index, index + 1)
