// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine

type PresetModel =
    { mutable SearchFor: string
      mutable ReplaceWith: string }

type BerkasExt = MKV | MP4 | Unsupported

type BerkasModel =
    { FilePath: string
      FileName: string
      Extension: BerkasExt
      mutable Title: string }

type ConfigModel = { mutable Presets: PresetModel seq }