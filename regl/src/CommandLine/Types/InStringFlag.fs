namespace Regl.CommandLine.Types

/// <summary>
/// 字符串类型标志，附带字符串值
/// </summary>
type InStringFlag(name, value) =
    interface IInFlag<string> with
        member f.name = name
        member f.value: string = value

        member f.tryParse arg1 arg2 =
            if arg1 <> name then None else Some(InStringFlag(name, arg2))

    new(name) = InStringFlag(name, "")