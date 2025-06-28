module Regl.CommandLine.Commands.GenCommand.Shared

open Regl.CommandLine.Commands.GenCommand.Types
/// the identifier for each source file's lines to be
/// identified as a code gen command
let mutable identifier = "//#!"
/// the context matchers for gen command
let mutable evcms: EnvironmentVariableContextMatcher list = []