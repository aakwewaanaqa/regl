namespace Regl.CommandLine.Builders

open Regl.CommandLine.Types

/// <summary>
/// 表示创建命令行参数解析器的构建器
/// 支持必需参数、必需参数和可选参数
/// </summary>
type CommandBuilder(name: string, execution: ParseResult option -> unit) =
    /// 命令名称
    member this.name = name
    /// 命令用法
    member val usage = None with get, set
    /// 按顺序排在所有标志之前的必需参数数量
    member val requiredParamsCount = 0 with get, set
    /// 在参数之后以任意顺序出现但必需的标志
    member val requiredFlags = list<IFlag>.Empty with get, set
    /// 在参数之后以任意顺序出现但可选的标志
    member val optionalFlags = list<IFlag>.Empty with get, set

    /// 使tryParser返回Some或None
    member private this.parser(argv: string array) =
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
        if argv.Length < (1 + this.requiredParamsCount + this.requiredFlags.Length) then
            None
        else if argv[0] <> name then
            None
        else if
            not (
                this.requiredFlags
                |> List.forall (fun f -> argv |> Array.exists (fun arg -> arg = f.name))
            )
        then
            None
        else

            let parameters = argv[1..][.. this.requiredParamsCount - 1]
            let flagArgs = argv[1..][this.requiredParamsCount ..]

            Some(
                { parameters = parameters
                  flags = flagArgs |> parseFlags }
            )

    member this.build() = {
            parse = this.parser
            usage = this.usage
            execute = execution
        }
