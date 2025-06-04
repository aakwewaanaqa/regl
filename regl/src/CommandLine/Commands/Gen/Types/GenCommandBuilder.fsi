namespace Regl.CommandLine.Commands.Gen.Types

open System.IO
open Regl.CommandLine.Types

type GenCommandBuilder =
    new : name : string * exe : (StringReader -> unit) -> GenCommandBuilder
    interface ICommandBuilder<GenCommandBody>