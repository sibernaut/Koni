﻿namespace Koni.Engine

open System
open System.IO.Abstractions
open System.Text.Json

module Configuration =
    let save config (filesystem: IFileSystem) =
        let jsonOptions = new JsonSerializerOptions(WriteIndented = true)
        let jsonStrings = JsonSerializer.Serialize(config, jsonOptions)
        let path = filesystem.Path.Combine(AppContext.BaseDirectory, "config.json")
        filesystem.File.WriteAllText(path, jsonStrings)

    let load (filesystem: IFileSystem) =
        let path = filesystem.Path.Combine(AppContext.BaseDirectory, "config.json")
        let isExist = filesystem.File.Exists(path)
        if not isExist then
            let config = { Presets = seq [ Preset.defaultPreset ]}
            save config filesystem
        let jsonStrings = filesystem.File.ReadAllText path
        JsonSerializer.Deserialize<ConfigModel> jsonStrings
