// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine

open System
open System.Collections.ObjectModel
open System.IO.Abstractions
open System.Text.Json

type Configuration = { Presets: ObservableCollection<Preset> }

module Configuration =
    let empty = { Presets = new ObservableCollection<Preset>() }
    let save config (filesystem: IFileSystem) =
        let jsonOptions = new JsonSerializerOptions(WriteIndented = true)
        let jsonStrings = JsonSerializer.Serialize(config, jsonOptions)
        let path = filesystem.Path.Combine(AppContext.BaseDirectory, "config.json")
        filesystem.File.WriteAllText(path, jsonStrings)
    let load (filesystem: IFileSystem) =
        let path = filesystem.Path.Combine(AppContext.BaseDirectory, "config.json")
        let isExist = filesystem.File.Exists(path)
        if not isExist then save empty filesystem
        let jsonStrings = filesystem.File.ReadAllText path
        JsonSerializer.Deserialize<Configuration> jsonStrings
