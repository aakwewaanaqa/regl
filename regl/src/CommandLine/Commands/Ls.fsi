module Regl.CommandLine.Commands.Ls

open Regl.CommandLine.Types

/// <summary>
/// List fils in current's directory.
/// </summary>
val public cmd : CommandBody

/// <summary>
/// Execute the ls command
/// </summary>
val private exe : result : ParseResult option -> unit