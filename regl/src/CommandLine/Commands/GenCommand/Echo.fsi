module Regl.CommandLine.Commands.GenCommand.Echo

open Regl.CommandLine.Types

/// <summary>
/// This executes the echo command
/// </summary>
val private exe: result: ParseResult option -> unit

/// <summary>
/// This echos the input text to Out
/// </summary>
val public cmd: CommandBody
