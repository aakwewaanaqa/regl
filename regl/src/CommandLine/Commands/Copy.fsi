module Regl.CommandLine.Commands.Copy

open Regl.CommandLine.Types

/// <summary>
/// Copy piped input to clipboard
/// </summary>
val public cmd : CommandBody

/// <summary>
/// Execute the copy command
/// </summary>
val private exe : result : ParseResult option -> unit