module Regl.CommandLine.Commands.GenCommand.Import

open Regl.CommandLine.Builders
open Regl.CommandLine.IO
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared

let rec subCmds =
    [| AddEvcm.cmd
       Copy.cmd
       Echo.cmd
       cmd
       SetEnvar.cmd
       Tpl.cmd
       UnsetEnvar.cmd |]

and exe (r : CommandParseResult) =
    let mutable identifier : string = ""
    let mutable buffer = r.getParam 0 |> ByFilePath |> LinesBuffer
    let iteri i (line : string) =
        buffer.index <- i

        if i = 0 then
            identifier <- line.TrimEnd ()
        elif line.Trim().StartsWith identifier then
            line
            |> _.Trim()
            |> _.Substring(identifier.Length)
            |> parseCommandLineArgs
            |> tryCommands (subCmds |> List.ofArray) 
            |> function
                | Ok () -> ()
                | Error ex -> debugLog $"regl gen -> import -> {ex}"

    buffer.iteriRest(iteri)

and cmd : CommandBody =
    let builder = CommandBuilder ("import", exe)
    builder.parameters <- [ Param ("text") ]
    builder.build ()
