module Regl.CommandLine.Commands.Copy

open Regl.CommandLine.Types

/// <summary>
/// Copies piped input to clipboard
/// </summary>
/// <usage>
/// regl copy
/// </usage>
val public cmd : CommandBody

/// <summary>
/// Execute the copy command
/// </summary>
val private exe : result : CommandParseResult -> unit