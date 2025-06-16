namespace Regl.CommandLine.Types

open System

type OnFlag (name, ?usage) =
    let usage = usage |> Option.defaultValue ""
    [<Obsolete>]
    member f.name = name
    [<Obsolete>]
    member f.parse(arg : string) =
        if name.Equals arg then
            { name = name ; value = OfBool true }
        else
            raise (Exception usage)

    override f.ToString() = name

    interface IFlag with
        member f.name = name
        member f.usage = usage
        member f.needInput = false
        member f.getVal _ = OfBool true
        member f.CompareTo(obj : obj) : int = $"{f}".CompareTo $"{obj}"
