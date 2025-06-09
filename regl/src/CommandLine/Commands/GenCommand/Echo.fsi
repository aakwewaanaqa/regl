module Regl.CommandLine.Commands.GenCommand.Echo

open Regl.CommandLine.Types

/// <summary>
/// This executes the echo command
/// </summary>
val private exe : CommandParseResult -> unit

/// <summary>
/// This echos the input text to Out
/// </summary>
/// <usage>
/// //#!echo &lt;text&gt;
/// </usage>
/// <remarks>
/// //#! is just the common way to mark
/// a line of regl gen source file command.
/// It can be anything. If it is the first line
/// of a source file
/// </remarks>
val public cmd: CommandBody
