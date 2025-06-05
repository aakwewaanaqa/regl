module Regl.CommandLine.Commands.Gen

open Regl.CommandLine.Types
open Regl.CommandLine.Builders
open Regl.CommandLine.Commands.GenCommand

let exe (result : ParseResult option) : unit =
    Implementation.exe(result)

let cmd =
    let builder = CommandBuilder("gen", exe)
    builder.usage <- Some "regl gen"
    builder.build ()
