module Regl.CommandLine.Commands.Gen

open System.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.GenCommand.Types

/// <summary>
/// From piped input as source text to execute generation commands.
/// </summary>
val cmd : CommandBody

/// <summary>
/// Execute the gen command
/// </summary>
val private exe : result : ParseResult option -> unit

val isCmd : string -> bool

val isNotCmd : string -> bool

val mutable commandIdentifier : string

val mutable evcms : EnvironmentVariableContextMatcher list