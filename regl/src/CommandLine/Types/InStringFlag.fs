namespace Regl.CommandLine.Types

open System

type InStringFlag (name, ?usage) =
    let usage = usage |> Option.defaultValue ""

    [<Obsolete>]
    member f.name = name

    [<Obsolete>]
    member f.parse (argName : string) (argVal : string) =
        if name.Equals argName then
            { name = name ; value = OfText argVal }
        else
            raise (Exception usage)

    override f.ToString() = name
    override f.GetHashCode() = HashCode.Combine name
    override f.Equals(b : obj) = f.GetHashCode() = b.GetHashCode ()

    interface IFlag with
        member f.name = name
        member f.usage = usage
        member f.needInput = true
        member f.getVal a = OfText a
        member f.CompareTo(obj : obj) : int = $"{f}".CompareTo ($"{obj}")
