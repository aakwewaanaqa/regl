module Regl.CommandLine.Commands.Gen

open System.IO
open Regl.CommandLine.IO
open Regl.CommandLine.Shared
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let mutable commandIdentifier = ""

let isCmd (atLine: string) =
    atLine.TrimStart().StartsWith(commandIdentifier)

let exe (result: ParseResult option) =

    let mutable beginning = ""
    let mutable copyLineCount = 0

    let copyLineCmd =
        let copyLineExe (result: ParseResult option) =
            match result with
            | Some result -> copyLineCount <- result.parameters[0] |> int
            | None -> ()

        let builder = CommandBuilder("copy", copyLineExe)
        builder.requiredParamsCount <- 1
        builder.build ()

    let genCmds = [ copyLineCmd ]

    InOut.In.rest()
    |> Seq.iteri (fun i line ->

        if i = 0 then
            beginning <- line
        elif line.Trim().StartsWith(beginning) then
            let genCmd = line.Trim().Substring(beginning.Length)
            let genArgv = genCmd.Split(" ")

            match genCmds |> List.tryFind (fun c -> c.parse (genArgv) |> _.IsSome) with
            | Some cmd -> cmd.parse genArgv |> cmd.execute
            | None -> ()
        else if copyLineCount > 0 then
            writeOutLine line
            copyLineCount <- copyLineCount - 1
        else
            ()
    )

let cmd =
    let builder = CommandBuilder("gen", exe)
    builder.usage <- Some "regl gen"
    builder.build ()
