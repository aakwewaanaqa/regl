namespace Regl.CommandLine.Types

/// <summary>
/// 开关类型标志，没有额外值
/// </summary>
type OnFlag(name) =
    interface IFlag with
        member f.name = name