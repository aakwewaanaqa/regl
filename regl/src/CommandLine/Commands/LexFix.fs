module Regl.CommandLine.Commands.LexFix

open System
open System.Text
open Regl.CommandLine.IO
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds

let cmdName = "lex-fix"

let cmdInfo = "lexically fixes stdin and writes to stdout"

let entry =
    let scopeFlag =
        StringFlag ("--scope", "match opening and closing character. ex: <> or {}")

    let exeWithScope : ArgBehaviour =
        fun dto ->
            In <- ReadonlyLinesBuffer ByConsoleIn

            let scopeVal = dto.flags.first<string> scopeFlag
            let opening = scopeVal[0]
            let closing = scopeVal[1]

            let mutable openCount = 0
            let builder = StringBuilder ()

            let rec loop (src : char list) =
                match src with
                | c :: rest ->
                    if c = opening then
                        openCount <- openCount + 1
                        builder.Append c |> ignore
                        loop rest
                    elif c = closing then
                        if openCount > 0 then
                            openCount <- openCount - 1
                            builder.Append c |> ignore

                        loop rest
                    else
                        builder.Append c |> ignore
                        loop rest
                | [] -> builder.ToString ()

            let result = loop (In.all.ToCharArray () |> List.ofArray)
            Out.all <- result

    CmdEntry (cmdName, cmdInfo)
    |> _.addEntry(
        ArgEntry("fix by scope")
        |> _.addFlag(scopeFlag)
        |> _.addBehaviour(exeWithScope)
    )
