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

let exe (r : CommandParseResult) =
    let echoMapper (line : string) =
        if line.TrimStart().StartsWith (echoIdentifier) then
            "echo \"" + line.TrimStart('#', '>').TrimEnd () + "\""
        else
            line

    InOut.In.filterRest isNotCmd (r.getParamT<int> 0)
    |> fun sequence -> ReadonlyLinesBuffer (BySeq sequence)
    |> _.all
    |> fun ctx -> evcms |> List.iter (fun m -> m.doMatch ctx)

    r.getParam 1
    |> ByFilePath
    |> LinesBuffer
    |> _.mapRest(echoMapper)
    |> _.executeInBash()
    |> _.Split("\n")
    |> Array.iter (fun l -> InOut.Out.appendLine l)

let cmd =
    let builder = CommandBuilder ("tpl", exe)
    builder.parameters <- [ Param("line-count"); Param("template-file") ]
    builder.usage <- tplUsage
    builder.build ()
