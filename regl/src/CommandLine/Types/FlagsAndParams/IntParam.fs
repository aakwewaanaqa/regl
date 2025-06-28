namespace Regl.CommandLine.Types.FlagsAndParams

type IntParam(name, ?usage) =
    interface IParam with
        member p.name = name
        member p.info = usage |> Option.defaultValue ""
        member p.getVal (arg : string) = OfInt(arg |> int)
        override p.CompareTo (obj : obj) : int =
            match obj with
            | :? IParam as op -> name.CompareTo op.name
            | obj -> name.CompareTo obj
            
    override p.ToString() : string =
        name