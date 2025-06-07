module Regl.CommandLine.Commands.GenCommand.SetEnvar

open Regl.CommandLine.Types

/// <summary>
/// Executes the set-envar command
/// </summary>
val private exe : result : ParseResult option -> unit

/// <summary>
/// Sets the environmental variable to a value
/// </summary>
/// <usage>
/// //#!set-envar &lt;VARIABLE-NAME&gt; &lt;VALUE&gt;
/// </usage>
/// <remarks>
/// //#! is just the common way to mark
/// a line of regl gen source file command.
/// It can be anything. If it is the first line
/// of a source file
/// </remarks>
val public cmd : CommandBody
