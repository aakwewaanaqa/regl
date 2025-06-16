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
    member val parameters : IParam list = []
    member val flags : IFlag list = []

module ArgEntry =
    let rec validate (va : ArgEntry) (args : Args) =
        let mutable flagVals : Map<IFlag, FlagVal> = Map<IFlag, FlagVal>(seq {})
        let mutable paramVals : string list = []
        try
            let hasRequirements = va.parameters.Length > 0 || va.flags.Length > 0
            guard (hasRequirements && args.length <= 0) $"validation named {va.name} has not enough args!"
            for flag in va.flags do
                args |> Args.getValue flag
            Ok ()
        with ex ->
            Error $"{ex}"