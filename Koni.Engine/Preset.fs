// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine

open System.Text.RegularExpressions

type Preset =
    { SearchFor: string
      ReplaceWith: string }

module Preset =
    let defaultPreset = 
        { SearchFor = @"^(.*?)\s-\s(\b(?!Opening|Ending|OVA\b|\d))"
          ReplaceWith = "$1: $2" }
    let apply preset input =
        Regex.Replace(input, preset.SearchFor, preset.ReplaceWith)

    module Collection =
        let apply presets input =
            let mutable output = input
            presets |> Seq.iter (fun p -> output <- apply p output)
            output