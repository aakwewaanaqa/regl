namespace Regl.CommandLine.Types.Arguments

open System.Collections.Generic
open Regl.CommandLine.Types
open Regl.CommandLine.Types.FlagsAndParams
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
    member val behaviour : ArgBehaviour = ignore with get, set

    member e.addParameter parameter =
        e.parameters <- e.parameters @ [ parameter ]
        e

    member e.addFlag flag =
        e.flags <- e.flags @ [ flag ]
        e

    member e.addFlags flags =
        e.flags <- e.flags @ flags
        e

    member e.addBehaviour behaviour =
        e.behaviour <- behaviour
        e

and ArgValidateDto =
    { flags : FlagValSet
      parameters : Dictionary<IParam, ArgVal>
      rem : Args }

and ArgBehaviour = ArgValidateDto -> unit

module ArgEntry =
    let rec validate (args : Args) (ae : ArgEntry) =
        try
            let hasRequirements = ae.parameters.Length > 0 || ae.flags.Length > 0
            guard (hasRequirements && args.length <= 0) $"validation named {ae.name} has not enough args!"

            let getFlagRem =
                let mutable rem = args

                let set = FlagValSet ()

                for flag in ae.flags do
                    let rec loopGetValue () =
                        try
                            let dto = rem |> Args.getValue flag |> guardResult
                            rem <- dto.rem
                            set.addVal flag dto.flagVal
                            loopGetValue ()
                        with ex ->
                            reraise ()

                    loopGetValue ()

                (set, rem)

            let getParamRem =
                let mutable _, rem = getFlagRem
                let mutable paramVals : Dictionary<IParam, ArgVal> = Dictionary<IParam, ArgVal> []

                for param in ae.parameters do
                    let dto = rem |> Args.getParam param |> guardResult
                    rem <- dto.rem
                    paramVals.Add (dto.param, dto.paramVal)

                (paramVals, rem)

            let flags, _ = getFlagRem
            let parameters, rem = getParamRem

            let dto =
                { flags = flags
                  parameters = parameters
                  rem = rem }

            let behaviour = ae.behaviour
            Ok (behaviour dto)
        with ex ->
            Error $"{ex}"

    let printHelp (va : ArgEntry) =
        $"""Entry {va.name} : {va.info} :
->  requires parameters {va.parameters |> List.map (fun p -> $"<{p.name}>")}
->  requires flags {va.flags |> List.map (fun p -> $"<{p.name}>")}
"""
