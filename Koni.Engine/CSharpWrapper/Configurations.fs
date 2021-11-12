// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine.Wrapper

open System.IO.Abstractions
open Koni.Engine

type Config(filesystem: IFileSystem) =
    let _filesystem = filesystem
    let mutable _item = Configuration.create Seq.empty<PresetModel>
    let mutable _presets = new Presets()
    member this.Item = _item
    member this.Presets
        with get() = _presets
        and set(value) = _presets <- value
    member this.Save() =
        let presets = this.Presets.Items |> Seq.map (fun p -> Preset.create p.SearchFor p.ReplaceWith)
        _item <- Configuration.updatePresets _item presets
        Configuration.save _item _filesystem
    member this.Load() =
        _item <- Configuration.load _filesystem
        this.Presets <- new Presets()
        _item.Presets |> Seq.iter (fun p -> this.Presets.Add(new Preset(p.SearchFor, p.ReplaceWith)))

    new() = Config(new FileSystem())