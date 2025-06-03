module regl.Commands

open System.Text
open System.Text.RegularExpressions
open Builders.Source
open System
open System.IO
open TextCopy
open regl.Builders.Source

let readIn () = Console.In.ReadToEnd()

/// Writes the specified string to the console's standard output stream.
/// This function takes a string as an argument and directly writes it to the standard output stream of the Console.
let writeOut (a: string) = Console.Out.Write(a)

/// This function takes a string argument and writes it to the standard output stream of the console, appending a new line at the end.
let writeOutLine (a: string) = Console.Out.WriteLine(a)

(*
      zh-TW: 將輸入的內容複製到剪貼簿
      en-US: Copy input content to clipboard
    *)
let copyCmd =
    let copyExe (result: ParseResult option) =
        match result with
        | Some result -> ClipboardService.SetText(readIn ())
        | None -> raise (Exception "copy can't be executed...")

    let builder = CommandBuilder("copy", copyExe)
    builder.usage <- Some "regl copy"
    builder.build ()


(*
      zh-TW: 使用指定的分隔符將輸入文本分割
      en-US: Split input text using specified delimiter
    *)
let splitCmd =
    let splitExe (result: ParseResult option) =
        match result with
        | Some result ->
            let out =
                Regex(result.parameters[0])
                |> _.Split(readIn ())
                |> Array.reduce (fun a b -> $"{a}\n{b}")

            writeOut out
        | None -> raise (Exception "split can't be executed...")

    let builder = CommandBuilder("split", splitExe)
    builder.requiredParamsCount <- 1
    builder.usage <- Some "regl split <DELIMITER>"
    builder.build ()

(*
      zh-TW: 使用正則表達式匹配文本並可選擇輸出格式
      en-US: Match text using regex with optional output format
    *)
let matchCmd =
    let matchExe (result: ParseResult option) =
        match result with
        | Some result ->
            let format =
                result.flags
                |> Array.tryFind (fun arg -> arg.name = "--format")
                |> Option.map (fun f -> f :?> IInFlag<string>)
                |> Option.bind (fun f -> Some f.value)
                |> Option.defaultValue "$0"

            let out =
                let regex = Regex(result.parameters[0])
                let matches = regex.Matches(readIn ())

                matches
                |> Seq.map (fun m ->
                    let mutable result = format

                    for i = 0 to m.Groups.Count - 1 do
                        result <- result.Replace($"${i}", m.Groups[i].Value)

                    result)
                |> Seq.reduce (fun a b -> $"{a}\n{b}")

            writeOut out
        | None -> raise (Exception "match can't be executed...")

    let builder = CommandBuilder("match", matchExe)
    builder.requiredParamsCount <- 1
    builder.optionalFlags <- [ InString("--format") ]
    builder.usage <- Some "regl match <REGEX> [--format <FORMAT>]"
    builder.build ()

(*
      zh-TW: 移除文本中的空行
      en-US: Remove empty lines from text
    *)
let removeEmptyCmd =
    let removeEmptyExe (result: ParseResult option) =
        match result with
        | Some _ ->
            let out =
                readIn ()
                |> _.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                |> Array.reduce (fun a b -> $"{a}\n{b}")

            writeOut out
        | None -> raise (Exception "removeEmpty can't be executed...")

    let builder = CommandBuilder("remove-empty", removeEmptyExe)
    builder.usage <- Some "regl remove-empty"
    builder.build ()

(*
      zh-TW: 列出當前目錄中的文件，可選遞迴搜索
      en-US: List files in current directory with optional recursive search
    *)
let lsCmd =
    let lsExe (result: ParseResult option) =
        match result with
        | Some result ->
            let isRecursive = result.flags |> Array.tryFind (fun f -> f.name = "-R") |> _.IsSome

            let searchOption =
                if isRecursive then
                    SearchOption.AllDirectories
                else
                    SearchOption.TopDirectoryOnly

            let out =
                Directory.GetCurrentDirectory()
                |> (fun pwd -> Directory.GetFiles(pwd, "", searchOption))
                |> Array.reduce (fun a b -> $"{a}\n{b}")

            writeOut out
        | None -> raise (Exception "ls can't be executed...")

    let builder = CommandBuilder("ls", lsExe)
    builder.optionalFlags <- [ OnFlag("-R") ]
    builder.usage <- Some "regl ls [-R]"
    builder.build ()

(*
      zh-TW: 將輸入內容寫入指定文件，可選追加模式
      en-US: Write input content to specified file with optional append mode
    *)
let toFileCmd =
    let toFileExe (result: ParseResult option) =
        match result with
        | Some result ->
            let isAppend =
                result.flags |> Array.tryFind (fun f -> f.name = "--append") |> _.IsSome

            let path = result.parameters[0]

            if isAppend then
                File.AppendAllText(path, readIn ())
            else
                File.WriteAllText(path, readIn ())
        | None -> raise (Exception "to-file can't be executed...")

    let builder = CommandBuilder("to-file", toFileExe)
    builder.usage <- Some "regl to-file <FILEPATH>"
    builder.requiredParamsCount <- 1
    builder.build ()

(*
      zh-TW: 根據特定格式生成內容的通用命令
      en-US: Generic command for generating content based on specific format
    *)
let genCmd =
    let genExe (result: ParseResult option) =

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

        readIn ()
        |> _.Split("\n")
        |> Array.iteri (fun i line ->

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

    let builder = CommandBuilder("gen", genExe)
    builder.usage <- Some "regl gen"
    builder.build ()

let cmds = [ copyCmd; splitCmd; matchCmd; removeEmptyCmd; toFileCmd; lsCmd; genCmd ]
