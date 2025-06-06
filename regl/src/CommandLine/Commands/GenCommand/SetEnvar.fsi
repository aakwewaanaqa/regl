module Regl.CommandLine.Commands.GenCommand.SetEnvar

open Regl.CommandLine.Types

/// <summary>
/// Executes the set-envar command
/// </summary>
val private exe : result : ParseResult option -> unit

/// <summary>
/// Sets the environmental variable to a value
/// </summary>
val public cmd : CommandBody
