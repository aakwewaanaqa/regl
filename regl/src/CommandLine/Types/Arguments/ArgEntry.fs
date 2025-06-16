namespace Regl.CommandLine.Types

open Regl.Exts

/// <summary>
/// Used to validate a command's args provision.
/// In case that a command has multiple choices to provide args.
/// The properties named parameters and flags are all required.
/// </summary>
type ArgEntry (name : string, ?info : string) =
    member this.name = name
    member this.info = info |> Option.defaultValue ""
    member val parameters : IParam list = [] with get, set
    member val flags : IFlag list = [] with get, set

[<Struct>]
type ArgValidateDto = {
    flags : Map<IFlag, FlagVal>
    parameters : Map<IParam, FlagVal>
    rem : Args
}

module ArgEntry =
    let rec validate (args : Args) (va : ArgEntry) =
        try
            let hasRequirements = va.parameters.Length > 0 || va.flags.Length > 0
            guard (hasRequirements && args.length <= 0) $"validation named {va.name} has not enough args!"
            let getFlagRem =
                let mutable rem = args
                let mutable flagVals : Map<IFlag, FlagVal> = Map<IFlag, FlagVal>(seq {})
                for flag in va.flags do
                    let dto = args |> Args.getValue flag |> guardResult
                    rem <- dto.rem
                    flagVals <- flagVals.Add (dto.flag, dto.flagVal)
                (flagVals, rem)
            let getParamRem =
                let mutable _, rem = getFlagRem
                let mutable paramVals : Map<IParam, FlagVal> = Map<IParam, FlagVal>(seq {})
                for param in va.parameters do
                    let dto = rem |> Args.getParam param |> guardResult
                    rem <- dto.rem
                    paramVals <- paramVals.Add (dto.param, dto.paramVal)
                (paramVals, rem)
            let flags, _ = getFlagRem
            let parameters, rem = getParamRem
            Ok { flags = flags; parameters = parameters; rem = rem }
        with ex ->
            Error $"{ex}"