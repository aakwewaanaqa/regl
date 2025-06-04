module Regl.CommandLine.Commands.Split

open Regl.CommandLine.Types

/// <summary>
/// Split piped input using specified delimiter
/// </summary>
val public cmd : CommandBody

/// <summary>
/// Execute the split command
/// </summary>
val private exe : result : ParseResult option -> unit