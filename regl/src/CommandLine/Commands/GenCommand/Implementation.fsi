module Regl.CommandLine.Commands.GenCommand.Implementation

open Regl.CommandLine.Types

val public subCmds : CommandBody array

val public exe : result: ParseResult option -> unit

val public cmd : CommandBody