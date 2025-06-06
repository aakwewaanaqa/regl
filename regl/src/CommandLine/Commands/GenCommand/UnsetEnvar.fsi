module Regl.CommandLine.Commands.GenCommand.UnsetEnvar

open Regl.CommandLine.Types

/// <summary>
/// This executes the unset-envar command
/// </summary>
val private exe : result : ParseResult option -> unit

/// <summary>
/// Unsets the environmental variable
/// </summary>
val public cmd : CommandBody
