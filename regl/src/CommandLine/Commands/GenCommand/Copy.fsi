module Regl.CommandLine.Commands.GenCommand.Copy

open Regl.CommandLine.Types

/// <summary>
/// This executes the copy command
/// </summary>
val private exe : CommandParseResult -> unit

/// <summary>
/// This copies the following count of lines to Out
/// </summary>
/// <usage>
/// //#!copy &lt;LINE-COUNT&gt;
/// </usage>
/// <remarks>
/// //#! is just the common way to mark
/// a line of regl gen source file command.
/// It can be anything. If it is the first line
/// of a source file
/// </remarks>
val public cmd: CommandBody
