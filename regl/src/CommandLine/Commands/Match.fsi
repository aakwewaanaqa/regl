module Regl.CommandLine.Commands.Match

open Regl.CommandLine.Types

/// <summary>
/// The usage of the match command
/// </summary>
val private usage : string

/// <summary>
/// Execute the match command
/// </summary>
val private exe : result : ParseResult option -> unit

/// <summary>
/// Match piped input with a regex pattern
/// </summary>
val cmd : CommandBody