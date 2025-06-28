namespace Regl.CommandLine.Types.Arguments

open System.Collections.Generic
open System.Text
open Regl.CommandLine.Types
open Regl.CommandLine.Types.FlagsAndParams
open Regl.Exts


/// <summary>
/// Used to validate a command's args provision.
/// In case that a command has multiple choices to provide args.
/// The properties named parameters and flags are all required.
/// </summary>
type ArgEntry (name : string) =
    member this.name = name
    member val parameters : IParam list = [] with get, set
    member val flags : IFlag list = [] with get, set
    member val behaviour : ArgBehaviour = ignore with get, set

    member e.addParameter parameter =
        e.parameters <- e.parameters @ [ parameter ]
        e

    member e.addFlag flag =
        e.flags <- e.flags @ [ flag ]
        e

    member e.addFlags (flags : IFlag list) =
        if flags.IsEmpty then
            e
        else
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
                let mutable set = FlagValSet ()

                for flag in ae.flags do
                    try
                        let dto = rem |> Args.getValue flag |> guardResult
                        rem <- dto.rem
                        set <- set.addVal flag dto.flagVal
                    with ex ->
                        reraise ()

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
            ae.behaviour({ flags = flags
                           parameters = parameters
                           rem = rem })

            Ok ()
        with ex ->
            Error $"{ex}"

    let getManual (ae : ArgEntry) =
        let mutable builder =
            StringBuilder()
            |> _.Append("        regl ")
            |> _.Append(ae.name)
        
        builder <-
            if ae.parameters.Length > 0 then
                builder.Append(' ')
                |> _.AppendJoin(" ", ae.parameters |> List.map (fun p -> $"<{p.name}>"))
            else
                builder
        
        builder <-
            if ae.flags.Length > 0 then
                builder.Append(' ')
                |> _.AppendJoin(" ", ae.flags |> List.map _.manual)
            else
                builder
                
        builder.ToString()