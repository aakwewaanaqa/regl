namespace Regl.CommandLine.Types.FlagsAndParams

open System.Text.RegularExpressions

type RegexParam(name : string, ?info : string) =
    interface IParam with
        member p.name = name
        member p.info = info |> Option.defaultValue ""
        member p.getVal (arg : string) = OfRegex(Regex arg)
        override p.CompareTo (obj : obj) : int =
            match obj with
            | :? IParam as op -> name.CompareTo op.name
            | obj -> name.CompareTo obj

    override p.ToString() : string =
        name