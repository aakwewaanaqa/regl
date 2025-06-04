namespace Regl.CommandLine.Types

/// <summary>
/// 表示命令行参数解析的结果
/// 包含位置参数和命名参数
/// </summary>
type ParseResult =
    { parameters: string array // 位置参数数组
      flags: IFlag array } // 命名标志数组及其值