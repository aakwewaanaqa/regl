module Regl.CommandLine.Commands.GenCommand.UnsetEnvar

open Regl.CommandLine.Types

/// <summary>
/// This executes the unset-envar command
/// </summary>
val private exe : result : ParseResult option -> unit

/// <summary>
/// Unsets the environmental variable
/// </summary>
/// <usage>
/// //#! unset-envar &lt;ENVAR-NAME&gt;
/// </usage>
/// <remarks>
/// //#! is just the common way to mark
/// a line of regl gen source file command.
/// It can be anything. If it is the first line
/// of a source file
/// </remarks>
val public cmd : CommandBody
