namespace Regl.CommandLine.Types

open System

/// <summary>
/// 表示命令行参数解析的结果
/// 包含位置参数和命名参数
/// </summary>
type ParseResult =
    { parameters: string array // 位置参数数组
      flags: IFlag array } // 命名标志数组及其值

    member r.hasFlag(name: string) =
        r.flags |> Array.exists (fun f -> f.name = name)

    member r.tryGetFlagValue (name: string) =
        r.flags
        |> Array.tryFind (fun f -> f.name = name)
        |> function
            | Some f -> Some (f :?> IInFlag<string>).value
            | None -> None

    member r.getParam (index: int) =
        if index >= r.parameters.Length then
            raise (Exception $"Parameter index {index} out of range")
        r.parameters[index]

    member r.getParamT<'a> (index: int) =
        r.getParam index |> (fun x -> Convert.ChangeType(x, typeof<'a>) :?> 'a)
