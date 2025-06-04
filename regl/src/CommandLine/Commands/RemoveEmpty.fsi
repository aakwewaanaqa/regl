module Regl.CommandLine.Commands.RemoveEmpty

open Regl.CommandLine.Types

/// <summary>
/// Remove empty piped input lines
/// </summary>
val public cmd : CommandBody

/// <summary>
/// Execute the remove-empty command
/// </summary>
val private exe : result : ParseResult option -> unit