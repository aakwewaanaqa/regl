module Regl.CommandLine.Commands.Copy

open Regl.CommandLine.Types

/// <summary>
/// Execute the copy command
/// </summary>
val exe : result : CommandParseResult -> unit

/// <summary>
/// Copies piped input to clipboard
/// </summary>
/// <usage>
/// regl copy
/// </usage>
val cmd : CommandBody
