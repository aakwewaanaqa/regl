module Regl.CommandLine.Commands.GenCommand.Copy

open Regl.CommandLine.Types

/// <summary>
/// This copies the following specified lines to Console.Out
/// </summary>
val public cmd : CommandBody

/// <summary>
/// This executes the copy command
/// </summary>
val private exe : result : ParseResult option -> unit