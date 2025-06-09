module Regl.CommandLine.Commands.ToFile

open Regl.CommandLine.Types

/// <summary>
/// Executes the to-file command
/// </summary>
val exe : CommandParseResult -> unit

/// <summary>
/// Writes piped input to a file
/// </summary>
val cmd : CommandBody
