module Regl.CommandLine.Types.Shared

open System

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

let argvParser (this: ICommandBuilder) (argv: string array) =
    /// 通过检查参数是否以-或--开头来确定它是否为标志
    let isFlag (arg: string) =
        arg.StartsWith("-") || arg.StartsWith("--")

    /// 获取给定索引处标志的关联值（如果存在）
    let getFlagValue (argv: string array) (index: int) =
        if index + 1 < argv.Length && not (isFlag argv[index + 1]) then
            Some argv[index + 1]
        else
            None

    let parseFlags (args: string array) =
        args
        |> Array.mapi (fun i arg ->
            if isFlag arg then
                match getFlagValue args i with
                | Some value ->
                    match this.requiredFlags @ this.optionalFlags |> List.tryFind (fun f -> f.name = arg) with
                    | Some flag ->
                        match flag with
                        | :? IInFlag<string> as inString ->
                            match inString.tryParse arg value with
                            | Some parsedFlag -> Some(parsedFlag :> IFlag)
                            | None -> None
                        | _ -> Some(OnFlag(arg))
                    | None -> None
                | None -> Some(OnFlag(arg))
            else
                None)
        |> Array.choose id

    // 根据命令的要求尝试解析命令行参数
    if argv.Length < (1 + this.requiredParamCount + this.requiredFlags.Length) then
        None
    else if argv[0] <> this.name then
        None
    else if
        not (
            this.requiredFlags
            |> List.forall (fun f -> argv |> Array.exists (fun arg -> arg = f.name))
        )
    then
        None
    else

        let parameters = argv[1..][.. this.requiredParamCount - 1]
        let flagArgs = argv[1..][this.requiredParamCount ..]

        Some(
            { parameters = parameters
              flags = flagArgs |> parseFlags }
        )
