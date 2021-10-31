// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine

open System
open System.Diagnostics
open System.IO
open System.IO.Abstractions

module Berkas =
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

    let save berkas =
        match berkas.Extension with
        | MP4 ->
            let tfile = TagLib.File.Create berkas.FilePath
            tfile.Tag.Title <- berkas.Title
            tfile.Save()
        | MKV ->
            let startinfo =
                new ProcessStartInfo(
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = "mkvpropedit.exe",
                    Arguments = sprintf "--edit info --set title=\"%s\" \"%s\"" berkas.Title berkas.FilePath
                )
            let ps = Process.Start(startinfo)
            ps.Start() |> ignore
            ps.WaitForExit()
        | Unsupported -> 
            NotImplementedException() |> ignore

module BerkasBerkas =
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
                |> Seq.map (fun b -> Berkas.create b presets filesystem)
            | false ->
                match isVideo path with
                | true -> seq [ Berkas.create path presets filesystem ]
                | false -> seq []

        items
        |> Seq.map (fun x -> getFiles x)
        //|> Seq.filter (fun x -> x <> seq [])
        |> Seq.concat