module Regl.CommandLine.Commands.GenCommand.Implementation

open Regl.CommandLine.Types

val cmds : CommandBody list

val exe : result: ParseResult option -> unit