module Regl.CommandLine.Commands.Shared

open System
open Regl.CommandLine.Types

let tryCommands (argv: string array) (cmds: CommandBody array) =
    try
        cmds
        |> Array.tryFind (fun cmd -> cmd.parse argv |> Option.isSome)
        |> function
            | Some cmd ->
                cmd.execute (cmd.parse argv)
                0
            | None ->
                printfn "Available commands:"
                cmds |> Array.choose _.usage |> Array.iter (printfn "%s")
                1
    with ex ->
        printfn $"Error: {ex}"
        1

let hasFlag (result: ParseResult option) (name: string) =
    match result with
    | Some r -> r.flags |> Array.exists (fun f -> f.name = name)
    | None -> false

let tryGetFlagValue (result: ParseResult option) (name: string) =
    match result with
    | Some r ->
        r.flags
        |> Array.tryFind (fun f -> f.name = name)
        |> function
            | Some f -> Some (f :?> IInFlag<string>).value
            | None -> None
    | None -> None

let getParam (result: ParseResult option) (index: int) =
    match result with
    | Some r ->
        if index >= r.parameters.Length then
            raise (Exception $"Parameter index {index} out of range")

        r.parameters[index]
    | None -> raise (Exception "No parse result available")
