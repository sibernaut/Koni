namespace Koni.Engine.Wrapper

open Koni.Engine

type Config(filesystem) =
    let _filesystem = filesystem
    let mutable _item = { Presets = Seq.empty<PresetModel> }
    let mutable _presets = new Presets()
    member this.Presets
        with get() = _presets
        and set(value) = _presets <- value
    member this.Save() =
        let presets =
            this.Presets.Items
            |> Seq.map (fun x -> 
                { SearchFor = x.SearchFor
                  ReplaceWith = x.ReplaceWith })
        _item.Presets <- presets
        Configuration.save _item _filesystem
    member this.Load() =
        _item <- Configuration.load _filesystem
        this.Presets <- new Presets()
        _item.Presets
        |> Seq.iter (fun x -> this.Presets.Add(x.SearchFor, x.ReplaceWith))
