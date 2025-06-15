namespace Regl.CommandLine.Types

open Regl.Exts

/// <summary>
/// Used to validate a command's args provision.
/// In case that a command has multiple choices to provide args.
/// The properties named parameters and flags are all required.
/// </summary>
type ArgValidation (name : string, ?info : string) =
    member this.name = name
    member this.info = info |> Option.defaultValue ""
    member val parameters : IParam list = []
    member val flags : IFlag list = []

module ArgValidation =
    let rec validate (va : ArgValidation) (args : LineArgs) =
        let mutable flagVals : Map<IFlag, FlagVal> = Map<IFlag, FlagVal>(seq {})
        try
            let hasRequirements = va.parameters.Length > 0 || va.flags.Length > 0
            guard (hasRequirements && args.length <= 0) $"validation named {va.name} has not enough args!"
            for flag in va.flags do
                if flag.hasVal then
                    let v = guardResult (args |> LineArgs.getValue flag)
                    flagVals <- flagVals.Add(flag, flag.getVal v)
                else
                    guardResult (args |> LineArgs.hasFlag flag)
                    flagVals <- flagVals.Add(flag, flag.getVal "")
            Ok ()
        with ex ->
            Error $"{ex}"