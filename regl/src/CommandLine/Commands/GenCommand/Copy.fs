module Regl.CommandLine.Commands.GenCommand.Copy

open System.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Types.Shared
open Regl.CommandLine.Commands.GenCommand.Types

let mutable lineCount : int = 0

let exe (result: ParseResult option) (sourceFile: StringReader) =
    lineCount <- getParam result 0 |> int

let cmd =
    let builder = GenCommandBuilder("copy", exe)
    builder._requiredParamCount <- 1
    builder :> ICommandBuilder<GenCommandBody> |> _.build()