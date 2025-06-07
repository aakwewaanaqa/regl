module Regl.CommandLine.Commands.GenCommand.Tpl

open System
open System.Diagnostics
open System.IO
open Regl.CommandLine.Builders
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Commands.GenCommand.Shared
open Regl.Lang

let echoIdentifier = "#>"

let exe (result: ParseResult option) =
    let echoMapper (line: string) =
        if line.TrimStart().StartsWith(echoIdentifier) then
            "echo \"" + line.TrimStart('#', '>').TrimEnd() + "\""
        else
            line

    match result with
    | Some r ->
        InOut.In.filterRest isNotCmd (r.getParamT<int> 0)
        |> fun sequence -> ReadonlyLinesBuffer(BySeq sequence)
        |> _.all
        |> fun ctx -> evcms |> List.iter (fun m -> m.doMatch ctx)

        r.getParam 1
        |> ByFilePath
        |> LinesBuffer
        |> _.mapRest(echoMapper)
        |> _.executeInBash()
        |> fun output -> InOut.Out.all <- output
    | None -> raise (Exception tplUsage)


let cmd =
    let builder = CommandBuilder("tpl", exe)
    builder.requiredParamsCount <- 2
    builder.usage <- Some tplUsage
    builder.build ()
