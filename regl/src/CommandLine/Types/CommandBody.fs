namespace Regl.CommandLine.Types
/// <summary>
/// 表示命令的执行体
/// </summary>
type CommandBody = {
      parse: string array -> ParseResult option
      execute: ParseResult option -> unit
      usage: string option
    }