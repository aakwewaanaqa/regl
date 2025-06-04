module Regl.CommandLine.Commands.ToFile

open Regl.CommandLine.Types

/// <summary>
/// Write piped input to a file
/// </summary>
val public cmd : CommandBody

/// <summary>
/// Execute the to-file command
/// </summary>
val private exe : result : ParseResult option -> unit