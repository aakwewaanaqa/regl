namespace Regl.CommandLine.Types

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
        let hasRequirements = va.parameters > 0 || va.flags > 0
        if hasRequirements && args.length <= 0 then
            Error $"validation named {va.name} has not enough args!"
        elif hasRequirements then
            if va.flags.Length > 0 then
                match va.flags.Head with
                | :? OnFlag f -> args.