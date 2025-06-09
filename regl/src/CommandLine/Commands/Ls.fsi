module Regl.CommandLine.Commands.Ls

open Regl.CommandLine.Types

/// <summary>
/// Execute the ls command
/// </summary>
val exe : result : CommandParseResult -> unit

/// <summary>
/// List fils in current's directory.
/// </summary>
val cmd : CommandBody
