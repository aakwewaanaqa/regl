module Regl.CommandLine.Commands.GenCommand.Shared

open Regl.CommandLine.Commands.GenCommand.Types

let mutable identifier = "//#!"

let isCmd (line: string) =
    line.TrimStart().StartsWith identifier

let isNotCmd (line: string) =
    not (isCmd line)

let mutable evcms: EnvironmentVariableContextMatcher list = []