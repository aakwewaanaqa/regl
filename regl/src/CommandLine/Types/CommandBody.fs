namespace Regl.CommandLine.Types
/// <summary>
/// 表示命令的执行体
/// </summary>
type CommandBody = {
      parse: string list -> CommandParseResult
      execute: CommandParseResult option -> unit
      usage: string
    }