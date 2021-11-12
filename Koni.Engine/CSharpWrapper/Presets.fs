// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine.Wrapper

open System.Collections.ObjectModel
open Koni.Engine

type Preset(search, replace) = 
    let mutable _item = Preset.create search replace
    member this.Item = _item
    member this.SearchFor = _item.SearchFor
    member this.ReplaceWith = _item.ReplaceWith
    static member Default = 
        let item = Preset.defaultPreset
        new Preset(item.SearchFor, item.ReplaceWith)

type Presets() =
    let _items = new ObservableCollection<Preset>()    
    let _items' = _items |> Seq.map (fun i -> Preset.create i.SearchFor i.ReplaceWith)
    member this.Items = _items
    member this.Apply(input) = Preset.Collection.apply _items' input
    member this.Add(preset) = _items.Add(preset)
    member this.Update(index, preset) = _items.[index] <- preset
    member this.Remove(item) = _items.Remove(item)
    member this.MoveUp(index) = if not (index = 0) then _items.Move(index, index - 1)
    member this.MoveDown(index) = if not (index = _items.Count - 1) then _items.Move(index, index + 1)
