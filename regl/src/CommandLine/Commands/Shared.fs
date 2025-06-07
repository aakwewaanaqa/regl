module Regl.CommandLine.Commands.Shared

open System
open System.Text.RegularExpressions
open Regl.CommandLine.Types

let ternary (flag: bool) a b = if flag then a else b

let tryCommands (argv: string array) (cmds: CommandBody array) =
    try
        cmds
        |> Array.tryFind (fun cmd -> cmd.parse argv |> Option.isSome)
        |> function
            | Some cmd ->
                cmd.execute (cmd.parse argv)
                0
            | None ->
                printfn "Available commands:"
                cmds |> Array.choose _.usage |> Array.iter (printfn "%s")
                1
    with ex ->
        printfn $"Error: {ex}"
        1

let hasFlag (result: ParseResult option) (name: string) =
    match result with
    | Some r -> r.flags |> Array.exists (fun f -> f.name = name)
    | None -> false

let tryGetFlagValue (result: ParseResult option) (name: string) =
    match result with
    | Some r ->
        r.flags
        |> Array.tryFind (fun f -> f.name = name)
        |> function
            | Some f -> Some (f :?> IInFlag<string>).value
            | None -> None
    | None -> None

let getParam (result: ParseResult option) (index: int) =
    match result with
    | Some r ->
        if index >= r.parameters.Length then
            raise (Exception $"Parameter index {index} out of range")

        r.parameters[index]
    | None -> raise (Exception "No parse result available")

let getParamT<'a> (result: ParseResult option) (index: int) =
    getParam result index |> (fun x -> Convert.ChangeType(x, typeof<'a>) :?> 'a)

let formatMatch (m: Match) (format: string) =
    let mutable formatted = format

    m.Groups
    |> Seq.iteri (fun i g ->
        if g.Success then
            formatted <- formatted.Replace($"${i}", g.Value))

    formatted

let parseCommandLineArgs (commandLine: string) =
    let rec parseQuoted (chars: char list) (current: string) (result: string list) (inQuote: bool) (escaping: bool) =
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

    parseQuoted (commandLine.ToCharArray() |> List.ofArray) "" [] false false
    |> Array.ofList
