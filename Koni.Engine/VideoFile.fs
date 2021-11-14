// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine

open System
open System.Diagnostics
open System.IO
open System.IO.Abstractions

module VideoFile =
    let create path config (filesystem: IFileSystem) =
        let ext =
            match filesystem.Path.GetExtension(path) with
            | ".mp4" -> MP4
            | ".mkv" -> MKV
            | _ -> Unsupported
        let title = 
            filesystem.Path.GetFileNameWithoutExtension(path)
            |> Preset.Collection.apply config.Presets
        { FileSystem = filesystem
          Config = config
          FilePath = path
          FileName = filesystem.Path.GetFileName(path)
          Folder = filesystem.Path.GetDirectoryName(path)
          Extension = ext
          Title = title }
    let updateConfig item config = { item with Config = config }
    let updateTitle item title = { item with Title = title }
    let resetTitle item =
        let filesystem = item.FileSystem
        let presets = item.Config.Presets
        let title = 
            filesystem.Path.GetFileNameWithoutExtension(item.FilePath)
            |> Preset.Collection.apply presets
        { item with Title = title }
    let save item =
        match item.Extension with
        | MP4 ->
            let tfile = TagLib.File.Create item.FilePath
            tfile.Tag.Title <- item.Title
            tfile.Save()
        | MKV ->
            let startinfo =
                new ProcessStartInfo(
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = "mkvpropedit.exe",
                    Arguments = sprintf "--edit info --set title=\"%s\" \"%s\"" item.Title item.FilePath
                )
            let ps = Process.Start(startinfo)
            ps.Start() |> ignore
            ps.WaitForExit()
        | Unsupported -> 
            NotImplementedException() |> ignore

    module Collection =
        let create items config (filesystem: IFileSystem) =
            let isFolder item = filesystem.File.GetAttributes(item).HasFlag(FileAttributes.Directory)
            let isVideo path = 
                let file = filesystem.Path.GetExtension(path)
                file = ".mp4" || file = ".mkv"
            let getFiles path =
                match isFolder path with
                | true ->
                    filesystem.Directory.EnumerateFiles(path)
                    |> Seq.filter isVideo
                    |> Seq.map (fun b -> create b config filesystem)
                | false ->
                    match isVideo path with
                    | true -> seq [ create path config filesystem ]
                    | false -> seq []
            items
            |> Seq.map (fun x -> getFiles x)
            |> Seq.concat