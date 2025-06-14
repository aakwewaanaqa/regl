namespace Regl.CommandLine.Types

type Param(name, ?usage) =
    interface IParam with
        member p.name = name
        member p.usage = usage |> Option.defaultValue "..."
        member p.parse (arg : string) = arg
