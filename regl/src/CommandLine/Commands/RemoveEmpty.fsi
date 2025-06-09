module Regl.CommandLine.Commands.RemoveEmpty

open Regl.CommandLine.Types

/// <summary>
/// Execute the remove-empty command
/// </summary>
val exe : result : CommandParseResult -> unit

/// <summary>
/// Remove empty piped input lines
/// </summary>
val cmd : CommandBody
