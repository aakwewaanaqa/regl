module Regl.CommandLine.Commands.Match

open Regl.CommandLine.Types

/// <summary>
/// Execute the match command
/// </summary>
val exe : CommandParseResult -> unit

/// <summary>
/// Match piped input with a regex pattern
/// </summary>
val cmd : CommandBody