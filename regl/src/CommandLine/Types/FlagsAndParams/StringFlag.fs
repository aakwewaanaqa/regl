namespace Regl.CommandLine.Types

open System
open Regl.CommandLine.Types.FlagsAndParams

type StringFlag (name, ?info) =
    let info = info |> Option.defaultValue ""

    override f.ToString() = name
    override f.GetHashCode() = HashCode.Combine name
    override f.Equals(b : obj) = f.GetHashCode() = b.GetHashCode ()

    interface IFlag with
        member f.name = name
        member f.info = info
        member f.manual = $"{name} <string>"
        member f.needInput = true
        member f.getVal a = OfString a
        member f.CompareTo(obj : obj) : int = $"{f}".CompareTo ($"{obj}")
