module Regl.CommandLine.Commands.Match

open Regl.CommandLine.Types

/// <summary>
/// Match piped input with a regex pattern
/// </summary>
val public cmd : CommandBody

/// <summary>
/// Execute the match command
/// </summary>
val private exe : result : ParseResult option -> unit