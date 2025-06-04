namespace Regl.CommandLine.Commands.GenCommand.Types

open System.IO
open Regl.CommandLine.Types

type GenCommandBuilder =
    new: name: string * exe: (ParseResult option -> StringReader -> unit) -> GenCommandBuilder
    member _optionalFlags: list<IFlag> with get, set
    member _requiredFlags: list<IFlag> with get, set
    member _requiredParamCount: int with get, set
    member _usage: string with get, set
    interface ICommandBuilder<GenCommandBody>