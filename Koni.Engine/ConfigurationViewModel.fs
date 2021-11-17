// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine

open System.IO.Abstractions
open Koni.Engine

type Config(filesystem: IFileSystem) =
    let _filesystem = filesystem
    let mutable _item = Configuration.empty
    member this.Item = _item
    member this.Presets = this.Item.Presets
    member this.Save() = Configuration.save this.Item _filesystem
    member this.Load() = _item <- Configuration.load _filesystem
    member this.Add(preset) = this.Presets.Add(preset)
    member this.Update(index, preset) = this.Presets.[index] <- preset
    member this.Remove(item) = this.Presets.Remove(item)
    member this.MoveUp(index) = 
        if (index > 0) then 
            this.Presets.Move(index, index - 1)
    member this.MoveDown(index) = 
        if (index < this.Presets.Count - 1) then 
            this.Presets.Move(index, index + 1)

    new() = Config(new FileSystem())