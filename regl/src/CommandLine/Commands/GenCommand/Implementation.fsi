module Regl.CommandLine.Commands.GenCommand.Implementation

open Regl.CommandLine.Types

val subCmds : CommandBody array

val exe : result: ParseResult option -> unit

val cmd : CommandBody