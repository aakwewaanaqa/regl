module Regl.CommandLine.Commands.GenCommand.Copy

open Regl.CommandLine.Builders
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Commands.GenCommand.Shared

let rec exe (r: CommandParseResult) =
    if r.parameters.Length > 0 then
        for atLine in In.filterRest isNotCmd (r.getParamT<int> 0) do
            Out.appendLine atLine
    elif r.hasFlag "--start" then
        let rec loop (src : string list) =
            match src with
            | atLine :: rest ->
                if isNotCmd atLine then
                    Out.appendLine atLine
                    loop rest
                elif isCmd atLine then
                    let argv =
                        atLine.Trim().Substring(identifier.Length)
                        |> parseCommandLineArgs
                    if argv[0] = "copy" then
                        let isEnd =
                            argv.Tail
                            |> cmd.parse
                            |> _.hasFlag("--end")
                        if isEnd then
                            ()
                        else
                            loop rest
                    else
                        loop rest
            | [] -> ()
        loop (In.rest() |> List.ofSeq |> List.skip 1)
    else
        debugLog "regl gen -> copy -> needs [--start] or [--line-count]"

and cmd =
    let builder = CommandBuilder("copy", exe)
    builder.optionalFlags <- [
        OnFlag("--start")
        OnFlag("--end")
    ]
    builder.build()