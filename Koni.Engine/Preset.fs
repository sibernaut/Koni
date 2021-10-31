namespace Koni.Engine

open System.Text.RegularExpressions

module Preset =
    let defaultPreset = 
        { SearchFor = @"^(.*?)\s-\s(\b(?!Opening|Ending|OVA\b|\d))"
          ReplaceWith = "$1: $2" }

    let apply preset input =
        Regex.Replace(input, preset.SearchFor, preset.ReplaceWith)

module Presets =
    let apply presets input =
        let mutable output = input
        presets
        |> Seq.iter (fun p -> output <- Preset.apply p output)
        output