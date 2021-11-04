// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine

open System.IO.Abstractions

type PresetModel =
    { SearchFor: string
      ReplaceWith: string }

type BerkasExt = MKV | MP4 | Unsupported

type ConfigModel = { Presets: PresetModel seq }

type VideoFileModel =
    { FileSystem: IFileSystem
      Config: ConfigModel
      FilePath: string
      FileName: string
      Extension: BerkasExt
      Title: string }
