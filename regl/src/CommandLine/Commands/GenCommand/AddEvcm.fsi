module Regl.CommandLine.Commands.GenCommand.AddEvcm

open Regl.CommandLine.Types

/// <summary>
/// This adds an environmental variable matcher for tpl command when read on the source file
/// </summary>
val public cmd : CommandBody

/// <summary>
/// This executes the add-evcm command
/// </summary>
val private exe : result : ParseResult option -> unit