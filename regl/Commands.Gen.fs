module regl.Commands.Gen

open System
open System.Collections.Generic
open System.IO
open System.Text
open System.Text.RegularExpressions
open regl.Builders.Source
open regl.Commands.Shared

let mutable source: TextReader = null
let mutable tFile: TextReader = null

let readLine (reader: TextReader) count =
    let builder = StringBuilder()

    let rec read count =
        if count > 0 && not (isNull (reader)) then
            let line = reader.ReadLine()

            if not (isNull line) then
                builder.Append(line) |> ignore
                read (count - 1)
            else
                builder.ToString()
        else
            builder.ToString()

    read count

type ContextVariableMatcher(pattern: Regex, format: string, key: string) =
    member x.doMatch ctx =
        let mutable formatted = format

        for m in pattern.Matches(ctx) do
            m.Groups
            |> Seq.iteri (fun i g -> formatted <- formatted.Replace($"${i}", g.Value))

        setEnvar key formatted

let mutable matchers: ContextVariableMatcher list = []

let ctxVarCmd =
    let ctxVarExe (result: ParseResult option) =
        let pattern = Regex(getParam result 0)
        let format = getParam result 1
        let key = getParam result 2
        matchers <- matchers @ [ ContextVariableMatcher(pattern, format, key) ]

    let builder = CommandBuilder("ctx-var", ctxVarExe)
    builder.requiredParamsCount <- 3
    builder.build ()

/// ctxCmd or context command is the command in the source file for generation.
/// This command consists of a form of
let ctxCmd =
    let ctxExe (result: ParseResult option) =
        let ctx = readLine source (getParam result 0 |> int)
        matchers |> List.iter (fun m -> m.doMatch ctx)

        match tryGetFlagValue result "--template-path" with
        | Some v -> tFile <- new StringReader(File.ReadAllText v)
        | None -> ()

        let replaceVars (line: string) =
            Regex("[$]([a-zA-Z0-9_])")
            |> _.Replace(line, fun m -> getEnvar m.Groups[1].Value)

        let rec templateRec () =
            let mutable read = readLine tFile 1
            read <- replaceVars read
            if read.EndsWith("#") then
                let commands = read.Split("#")[1]
            else
                writeOutLine read
            ()

        templateRec ()

    let builder = CommandBuilder("ctx", ctxExe)
    builder.requiredParamsCount <- 1
    builder.optionalFlags <- [ InString("--template-path") ]
    builder.usage <- "
    
    "
    builder.build ()

let copyCmd =
    let copyExe (result: ParseResult option) =
        writeOutLine (readLine source (getParam result 0 |> int))

    let builder = CommandBuilder("copy", copyExe)
    builder.requiredParamsCount <- 1
    builder.build ()
