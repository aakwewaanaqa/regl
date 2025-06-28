namespace Regl.CommandLine.Types

open System
open Regl.CommandLine.Types.FlagsAndParams

type BoolFlag (name, ?info) =
    let info = info |> Option.defaultValue ""

    [<Obsolete>]
    member f.name =
        raise (InvalidOperationException "Obsolete")

    [<Obsolete>]
    member f.parse (arg : string) =
        raise (InvalidOperationException "Obsolete")

    override f.ToString () =
        name

    override f.GetHashCode () =
        HashCode.Combine name

    override f.Equals (b : obj) =
        f.GetHashCode () = b.GetHashCode ()

    interface IFlag with
        member f.name = name
        member f.info = info
        member f.manual = $"{name}"
        member f.needInput = false

        member f.getVal _ =
            OfBool true

        member f.CompareTo (obj : obj) : int =
            $"{f}".CompareTo $"{obj}"
