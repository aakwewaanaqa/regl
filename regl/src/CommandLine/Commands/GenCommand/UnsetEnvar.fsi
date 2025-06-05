module Regl.CommandLine.Commands.GenCommand.UnsetEnvar

open Regl.CommandLine.Types

/// <summary>
/// Unsets the environmental variable
/// </summary>
val public cmd : CommandBody

/// <summary>
/// This executes the unset-envar command
/// </summary>
val private exe : result : ParseResult option -> unit