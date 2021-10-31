namespace Koni.Engine

type PresetModel =
    { mutable SearchFor: string
      mutable ReplaceWith: string }

type BerkasExt = MKV | MP4 | Unsupported

type BerkasModel =
    { FilePath: string
      FileName: string
      Extension: BerkasExt
      Title: string }

type ConfigModel = { mutable Presets: PresetModel seq }