// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Koni.Engine

open System.Text.RegularExpressions

module Preset =
    let defaultPreset = 
        { SearchFor = @"^(.*?)\s-\s(\b(?!Opening|Ending|OVA\b|\d))"
          ReplaceWith = "$1: $2" }
    let create search replace =
        { SearchFor = search
          ReplaceWith = replace }
    let updateSearch preset search = { preset with SearchFor = search }
    let updateReplace preset replace = { preset with ReplaceWith = replace }
    let apply preset input =
        Regex.Replace(input, preset.SearchFor, preset.ReplaceWith)

module Presets =
    let apply presets input =
        let mutable output = input
        presets
        |> Seq.iter (fun p -> output <- Preset.apply p output)
        output