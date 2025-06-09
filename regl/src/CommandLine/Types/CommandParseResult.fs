namespace Regl.CommandLine.Types

open System

type CommandParseResult =
    { parameters: string list
      flags: FlagParseResult list }

    member r.hasFlag(name: string) =
        r.flags |> List.exists (fun f -> f.name = name)

    member r.tryGetFlagValue (name: string) =
        r.flags
        |> List.tryFind (fun f -> f.name = name)
        |> function
            | Some f -> Some f.value
            | None -> None

    member r.getParam (index: int) =
        if index >= r.parameters.Length then
            raise (Exception $"Parameter index {index} out of range")
        r.parameters[index]

    member r.getParamT<'a> (index: int) =
        r.getParam index |> (fun x -> Convert.ChangeType(x, typeof<'a>) :?> 'a)
