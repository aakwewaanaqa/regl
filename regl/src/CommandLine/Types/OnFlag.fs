namespace Regl.CommandLine.Types

open System

type OnFlag (name, ?usage) =
    let usage = usage |> Option.defaultValue ""
    member f.name = name
    member f.parse(arg : string) =
        if name.Equals arg then
            { name = name; value = OfBool true }
        else
            raise (Exception usage)

    override f.ToString() =
        f.name

    interface IFlag with
        member f.name = name
        member f.usage = usage
