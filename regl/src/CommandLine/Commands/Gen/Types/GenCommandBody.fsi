namespace Regl.CommandLine.Commands.Gen.Types

open System.IO
open Regl.CommandLine.Types

type GenCommandBody = {
    parse   : string array -> ParseResult option
    usage   : string
    execute : ParseResult option -> StringReader -> unit
}