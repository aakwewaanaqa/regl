namespace Regl.CommandLine.Types

/// <summary>
/// 带值的命令行参数标志接口
/// </summary>
type IInFlag<'a> =
    inherit IFlag
    abstract member value: 'a
    abstract member tryParse: string -> string -> IInFlag<'a> option