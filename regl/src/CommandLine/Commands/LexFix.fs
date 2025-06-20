module Regl.CommandLine.Commands.LexFix

open System
open System.Text
open Regl.CommandLine.Builders
open Regl.CommandLine.IO
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types

///TODO : remove
[<Obsolete>]
let exe (r : CommandParseResult) =
    In <- ReadonlyLinesBuffer(ByConsoleIn)
    if r.hasFlag "--scope" then
        let scopeVal = r.tryGetFlagValue("--scope").Value.ToString()
        let opening  = scopeVal[0]
        let closing  = scopeVal[1]

        let mutable openCount = 0
        let builder = StringBuilder()
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
            | [] -> builder.ToString()
        let result = loop (In.all.ToCharArray() |> List.ofArray)
        Out.all <- result
///TODO : remove
[<Obsolete>]
let cmd : CommandBody =
    let builder = CommandBuilder ("lex-fix", exe)
    builder.optionalFlags <- [ InStringFlag "--scope" ]
    builder.build ()

//TODO : write entry