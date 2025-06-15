namespace Regl.CommandLine.Types

open System

type InStringFlag (name, ?usage) =
    let usage = usage |> Option.defaultValue ""
    member f.name = name
    member f.parse (argName : string) (argVal : string) =
        if name.Equals argName then
            { name = name ; value = OfText argVal }
        else
            raise (Exception usage)

    interface IFlag with
        member f.name = name
        member f.usage = usage
        member f.hasVal = true
        member f.getVal a = OfText a
        member f.CompareTo (obj: obj): int =
            $"{f}".CompareTo($"{obj}")