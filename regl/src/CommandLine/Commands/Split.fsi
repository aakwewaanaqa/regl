module Regl.CommandLine.Commands.Split

open Regl.CommandLine.Types

/// <summary>
/// Execute the split command
/// </summary>
val exe : result : ParseResult option -> unit

/// <summary>
/// Splits piped input using specified delimiter
/// then outputs them into lines
/// </summary>
/// <usage>
/// regl split &lt;DELIMITER&gt;
/// </usage>
val cmd : CommandBody
