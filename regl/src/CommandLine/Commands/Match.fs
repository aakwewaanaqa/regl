module Regl.CommandLine.Commands.Match

open System
open System.Text.RegularExpressions
open Regl.Exts
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds

let cmdName = "match"

let cmdInfo =
    "Matches the whole stdin with regex pattern then writes all matches to stdout"

let entry =

    let regexParam = Param ("the regex pattern to match with")

    let formatFlag =
        StringFlag (
            "--format",
            "the format to output every match. Like `$1` will print the first captured group. Or, `$0` will print the whole match. Even `tag $1` will insert text `tag ` then the first captured group."
        )

    let exeMatch : ArgBehaviour =
        fun dto ->
            InOut.In <- ReadonlyLinesBuffer (ByConsoleIn)
            let pattern = dto.parameters[regexParam] |> _.ToString()

            let format =
                if dto.flags.containsFlag formatFlag then
                    Some dto.flags[formatFlag]
                else
                    None

            pattern |> Regex |> _.Matches(InOut.In.all)
            |>?? (format.IsSome, Seq.map (fun m -> formatMatch m (format.Value.ToString ())), Seq.map _.Value)
            |> List.ofSeq
            |> fun lines -> InOut.Out.lines <- lines

    CmdEntry (cmdName, cmdInfo)
    |> _.addEntry(
        ArgEntry "Matches the whole stdin"
        |> _.addParameter(regexParam)
        |> _.addBehaviour(exeMatch)
    )
