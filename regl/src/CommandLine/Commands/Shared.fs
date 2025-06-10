module Regl.CommandLine.Commands.Shared

open System.Text
open System.Text.RegularExpressions
open Regl.CommandLine.Types

let ternary (flag : bool) a b = if flag then a else b

let isQuoted (str : string) =
    str.StartsWith '"' && str.EndsWith '"'

let tryCommands (argv : string list) (cmds : CommandBody list) =
    try
        cmds
        |> List.find (fun cmd -> cmd.name.Equals argv[0])
        |> fun cmd -> cmd.execute (cmd.parse argv.Tail)

        Ok ()
    with ex ->
        Error ex.Message

let formatMatch (m : Match) (format : string) =
    let mutable formatted = format

    m.Groups
    |> Seq.iteri (fun i g ->
        if g.Success then
            formatted <- formatted.Replace ($"${i}", g.Value))

    formatted

let parseCommandLineArgs (commandLine : string) =
    let mutable result : string list = []
    let mutable quoting : char = ' '
    let mutable escaping : bool = false
    let builder = StringBuilder ()

    let isLongFlag () = builder.ToString().StartsWith("--")
    let isInQuote () = quoting = ''' || quoting = '"'
    let isOfQuote (c : char) = quoting = c

    let rec parse (chars : char list) =
        match chars with
        | '\t'
        | ' ' as c :: rest ->
            if escaping || isInQuote () then
                builder.Append c |> ignore
                escaping <- false
            else if builder.Length > 0 then
                result <- result @ [ builder.ToString () ]
                builder.Clear () |> ignore

            parse rest
        | '=' as c :: rest ->
            if escaping then
                builder.Append c |> ignore
                escaping <- false
            elif isLongFlag() then
                if isInQuote () then
                    builder.Append c |> ignore
                else
                    result <- result @ [ builder.ToString () ]
                    builder.Clear () |> ignore
            else
                builder.Append c |> ignore

            parse rest
        | ''' as c :: rest ->
            if escaping then
                builder.Append c |> ignore
                escaping <- false
            else
                if isOfQuote c then
                    quoting <- ' '
                elif isInQuote () then
                    builder.Append c |> ignore
                else
                    quoting <- c

            parse rest
        | '"' as c :: rest ->
            if escaping then
                builder.Append c |> ignore
                escaping <- false
            else
                if isOfQuote c then
                    quoting <- ' '
                elif isInQuote () then
                    builder.Append c |> ignore
                else
                    quoting <- c

            parse rest
        | '\\' as c :: rest ->
            if escaping then
                builder.Append c |> ignore
                escaping <- false
            else
                escaping <- true

            parse rest
        | c :: rest ->
            builder.Append c |> ignore
            parse rest
        | [] ->
            if builder.Length > 0 then
                result <- result @ [ builder.ToString () ]
            else
                ()

    parse (commandLine.ToCharArray () |> List.ofArray)
    result
