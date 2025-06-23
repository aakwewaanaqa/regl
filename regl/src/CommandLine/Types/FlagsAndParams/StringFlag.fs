namespace Regl.CommandLine.Types

open System
open Regl.CommandLine.Types.FlagsAndParams

type StringFlag (name, ?usage) =
    let usage = usage |> Option.defaultValue ""

    [<Obsolete>]
    member f.name =
        raise (InvalidOperationException "Obsolete")

    [<Obsolete>]
    member f.parse (argName : string) (argVal : string) =
        raise (InvalidOperationException "Obsolete")

    override f.ToString() = name
    override f.GetHashCode() = HashCode.Combine name
    override f.Equals(b : obj) = f.GetHashCode() = b.GetHashCode ()

    interface IFlag with
        member f.name = name
        member f.usage = usage
        member f.needInput = true
        member f.getVal a = OfString a
        member f.CompareTo(obj : obj) : int = $"{f}".CompareTo ($"{obj}")
