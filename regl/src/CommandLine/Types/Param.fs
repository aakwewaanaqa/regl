namespace Regl.CommandLine.Types

type Param(name, ?usage) =
    interface IParam with
        member p.name = name
        member p.usage = usage |> Option.defaultValue "..."
        member p.parse (arg : string) = arg
        member p.getVal (arg : string) = OfText arg
        override p.CompareTo (obj : obj) : int =
            match obj with
            | :? IParam as op -> name.CompareTo op.name
            | obj -> name.CompareTo obj