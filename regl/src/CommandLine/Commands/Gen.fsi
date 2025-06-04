module Regl.CommandLine.Commands.Gen

open System.IO
open Regl.CommandLine.Types

/// <summary>
/// From piped input as source text to execute generation commands.
/// </summary>
val public cmd : CommandBody

/// <summary>
/// Execute the gen command
/// </summary>
val private exe : result : ParseResult option -> unit

val public isCmd : string -> bool