// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine

open System.Diagnostics
open System.IO
open System.IO.Abstractions

type Video =
    { Path: string
      FileName: string
      Directory: string
      Title: string 
      IsRenamed: bool 
      IsSaved: bool }

module Video =
    let getTitle path config (filesystem: IFileSystem) = 
        let filename = filesystem.Path.GetFileNameWithoutExtension(path)
        Preset.Collection.apply config.Presets filename
    let create path config (filesystem: IFileSystem) =
        { Path = path
          FileName = filesystem.Path.GetFileName(path)
          Directory = filesystem.Path.GetDirectoryName(path)
          Title = getTitle path config filesystem 
          IsRenamed = false
          IsSaved = false }
    let updateTitle item title = { item with Title = title }
    let resetTitle item config (filesystem: IFileSystem) = 
        { item with 
            Title = getTitle item.Path config filesystem }
    let save item (filesystem: IFileSystem) =
        let ext = filesystem.Path.GetExtension(item.Path)
        match ext with
        | ".mp4" ->
            let tfile = TagLib.File.Create item.Path
            tfile.Tag.Title <- item.Title
            tfile.Save()
            { item with IsSaved = true }
        | ".mkv" ->
            let startinfo =
                new ProcessStartInfo(
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = "mkvpropedit.exe",
                    Arguments = sprintf "--edit info --set title=\"%s\" \"%s\"" item.Title item.Path
                )
            let ps = Process.Start(startinfo)
            ps.Start() |> ignore
            ps.WaitForExit()
            { item with IsSaved = true }
        | _ -> item

    module Collection =
        let create items config (filesystem: IFileSystem) =
            let isFolder item = 
                filesystem.File
                    .GetAttributes(item)
                    .HasFlag(FileAttributes.Directory)
            let isVideo path =
                match filesystem.Path.GetExtension(path) with
                | ".mp4" | ".mkv" -> true
                | _ -> false
            let getFiles path =
                match isFolder path with
                | true -> filesystem.Directory.EnumerateFiles(path)
                | false -> seq [ path ]
            items
            |> Seq.map (fun x -> getFiles x)
            |> Seq.concat
            |> Seq.filter isVideo
            |> Seq.map (fun f -> create f config filesystem)