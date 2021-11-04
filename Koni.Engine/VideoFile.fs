// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine

open System
open System.Diagnostics
open System.IO
open System.IO.Abstractions

module VideoFile =
    let create path presets (filesystem: IFileSystem) =
        let ext =
            match filesystem.Path.GetExtension(path) with
            | ".mp4" -> MP4
            | ".mkv" -> MKV
            | _ -> Unsupported
        let filename = filesystem.Path.GetFileName(path)
        let title = filesystem.Path.GetFileNameWithoutExtension(path)
        { FilePath = path
          FileName = filename
          Extension = ext
          Title = Presets.apply presets title }
    let update item title = { item with Title = title }
    let reset item presets (filesystem: IFileSystem) =
        let title = filesystem.Path.GetFileNameWithoutExtension(item.FilePath)
        { item with Title = Presets.apply presets title }
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

module VideoFiles =
    let create items presets (filesystem: IFileSystem) =
        let isFolder item = filesystem.File.GetAttributes(item).HasFlag(FileAttributes.Directory)
        let isVideo path = 
            let file = filesystem.Path.GetExtension(path)
            file = ".mp4" || file = ".mkv"
        let getFiles path =
            match isFolder path with
            | true ->
                filesystem.Directory.EnumerateFiles(path)
                |> Seq.filter isVideo
                |> Seq.map (fun b -> VideoFile.create b presets filesystem)
            | false ->
                match isVideo path with
                | true -> seq [ VideoFile.create path presets filesystem ]
                | false -> seq []

        items
        |> Seq.map (fun x -> getFiles x)
        |> Seq.concat