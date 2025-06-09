namespace Regl.CommandLine.Types

type CommandBody = {
      name: string
      parse: string list -> CommandParseResult
      execute: CommandParseResult -> unit
      usage: string
    }