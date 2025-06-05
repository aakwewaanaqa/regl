module Regl.CommandLine.Commands.GenCommand.Shared

open Regl.CommandLine.Commands.GenCommand.Types

val isCmd : string -> bool

val isNotCmd : string -> bool

val mutable identifier : string

val mutable evcms : EnvironmentVariableContextMatcher list