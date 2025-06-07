module Regl.CommandLine.Commands.ToFile

open Regl.CommandLine.Types

/// <summary>
/// Execute the to-file command
/// </summary>
val private exe : result : ParseResult option -> unit

/// <summary>
/// Writes piped input to a file
/// </summary>
val public cmd : CommandBody
