namespace Regl.CommandLine.Commands.Gen.Types

open System.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Types.Shared

type GenCommandBuilder(name: string, exe: StringReader -> unit) =
    member val _optionalFlags = list<IFlag>.Empty with get, set
    member val _requiredFlags = list<IFlag>.Empty with get, set
    member val _requiredParamCount = 0 with get, set
    member val _usage = "" with get, set

    interface ICommandBuilder<GenCommandBody> with
        member this.name = name
        member this.usage = this._usage
        member this.requiredParamCount = this._requiredParamCount
        member this.optionalFlags = this._optionalFlags
        member this.requiredFlags = this._requiredFlags

        member this.build() =
            { parse = argvParser this
              usage = this._usage
              execute = exe }