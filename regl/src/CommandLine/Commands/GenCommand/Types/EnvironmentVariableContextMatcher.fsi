namespace Regl.CommandLine.Commands.GenCommand.Types

open System.Text.RegularExpressions

/// <summary>
/// 环境变量上下文匹配器，用于基于正则表达式匹配从上下文中提取值并设置为环境变量
/// </summary>
type EnvironmentVariableContextMatcher =
    /// <summary>
    /// 初始化新的环境变量上下文匹配器实例
    /// </summary>
    /// <param name="pattern">用于匹配上下文的正则表达式模式</param>
    /// <param name="format">格式化字符串，用于从匹配中提取值，可使用 $0、$1 等替换组</param>
    /// <param name="envarName">要设置的环境变量名称</param>
    new : pattern : Regex * format : string * envarName : string -> EnvironmentVariableContextMatcher

    /// <summary>
    /// 对提供的上下文进行匹配，并将匹配结果设置为环境变量
    /// </summary>
    /// <param name="ctx">要匹配的上下文字符串</param>
    /// <returns>无返回值</returns>
    /// <remarks>
    /// 匹配的结果会根据指定的格式字符串进行格式化，然后设置为指定的环境变量。
    /// 如果正则表达式有多个匹配项，每个匹配项都会导致环境变量被设置，最后一个匹配将成为最终值。
    /// </remarks>
    member doMatch : ctx : string -> unit
