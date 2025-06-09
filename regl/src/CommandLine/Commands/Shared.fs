module Regl.CommandLine.Commands.Shared

open System
open System.Text.RegularExpressions
open Regl.CommandLine.Types

let ternary (flag : bool) a b = if flag then a else b

let isQuoted (str : string) =
    str.StartsWith ('"') && str.EndsWith ('"')

let tryCommands (argv : string list) (cmds : CommandBody list) =
    try
        cmds
        |> List.find (fun cmd -> cmd.name.Equals argv[0])
        |> fun cmd -> cmd.execute (cmd.parse argv.Tail)
        0
    with ex ->
        printfn $"Error: {ex}"
        1

let formatMatch (m : Match) (format : string) =
    let mutable formatted = format

    m.Groups
    |> Seq.iteri (fun i g ->
        if g.Success then
            formatted <- formatted.Replace ($"${i}", g.Value))

    formatted

let parseCommandLineArgs (commandLine : string) =
    let rec parseQuoted
        (chars : char list)
        (current : string)
        (result : string list)
        (inQuote : bool)
        (escaping : bool)
        =
        match chars, escaping, inQuote with
        // 结束条件：没有更多字符
        | [], false, false ->
            if current.Length > 0 then
                current :: result |> List.rev
            else
                result |> List.rev

        // 结束引号内的字符串（未转义的引号）
        | '"' :: rest, false, true -> parseQuoted rest current result false false

        // 开始引号（未转义的引号）
        | '"' :: rest, false, false -> parseQuoted rest current result true false

        // 处理转义字符
        | '\\' :: rest, false, _ -> parseQuoted rest current result inQuote true

        // 转义的特殊字符
        | c :: rest, true, _ -> parseQuoted rest (current + string c) result inQuote false

        // 参数间的空格（不在引号内）
        | ' ' :: rest, false, false ->
            let newResult = if current.Length > 0 then current :: result else result
            parseQuoted rest "" newResult false false

        // 常规字符
        | c :: rest, false, _ -> parseQuoted rest (current + string c) result inQuote false

    parseQuoted (commandLine.ToCharArray () |> List.ofArray) "" [] false false
