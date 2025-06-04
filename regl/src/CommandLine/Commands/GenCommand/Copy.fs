module Regl.CommandLine.Commands.GenCommand.Copy

open Regl.CommandLine.Builders
open Regl.CommandLine.Commands
open Regl.CommandLine.IO
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types
open Regl.CommandLine.Types.Shared

let exe (result: ParseResult option) =
    let readerCurrentLineIndex = In._index
    let wantCopyLineCount = getParam result 0 |> int
    let rec exeLoop index count =
        if count <= 0 then ()
        elif index > In._lines.Length then ()
        else
            let atLine = In._lines[index]
            if not (Gen.isCmd atLine) then
                Out.appendLine atLine
                exeLoop (index + 1) (count - 1)
            else
                exeLoop (index + 1) count
    exeLoop readerCurrentLineIndex wantCopyLineCount

let cmd =
    let builder = CommandBuilder("copy", exe)
    builder.requiredParamsCount <- 1
    builder.build()