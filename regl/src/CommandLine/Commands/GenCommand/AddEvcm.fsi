module Regl.CommandLine.Commands.GenCommand.AddEvcm

open Regl.CommandLine.Types

/// <summary>
/// This executes the add-evcm command
/// </summary>
val private exe: result: ParseResult option -> unit

/// <summary>
/// Adds an environmental variable matcher for tpl command when reading the source file
/// </summary>
/// <usage>
/// //#!add-evcm &lt;REGEX&gt; &lt;MATCH-OUTPUT-FORMAT&gt; &lt;ENVAR-NAME&gt;
/// </usage>
/// <remarks>
/// //#! is just the common way to mark
/// a line of regl gen source file command.
/// It can be anything. If it is the first line
/// of a source file
/// </remarks>
val public cmd: CommandBody
