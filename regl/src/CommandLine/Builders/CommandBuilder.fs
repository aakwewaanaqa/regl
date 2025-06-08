namespace Regl.CommandLine.Builders

open Regl.CommandLine.Types
open Regl.Lang.ExceptionLang

module CommandBuilder =
    let parseParameters (parameters : IParam list) (argv : string list) =
        let rec parse (parameters : IParam list) (argv : string list) (result : string list) =
            match parameters, argv with
            | p :: parameters, arg :: argv -> parse parameters argv (result @ [ p.parse arg ])
            | [], argv -> result, argv
            | parameters, [] -> raise parametersNotEnough

        parse parameters argv []

    let parseFlags (flags : IFlag list) (argv : string list) =
        let rec parse (flags : IFlag list) (argv : string list) (result : FlagParseResult list) =
            match flags with
            | :? OnFlag as f :: flags ->
                argv
                |> List.tryFindIndex (fun a -> a.Equals f.name)
                |> function
                    | Some index ->
                        let rest = argv |> List.removeAt index
                        let parsed = f.parse argv[index]
                        parse flags rest (result @ [ parsed ])
                    | None -> result, argv
            | :? InStringFlag as f :: flags ->
                argv
                |> List.tryFindIndex (fun a -> a.Equals f.name)
                |> Option.filter (fun index -> index + 1 < argv.Length)
                |> Option.filter (fun index -> not (argv[index + 1].StartsWith ("-")))
                |> function
                    | Some index ->
                        let rest = argv |> List.removeManyAt index 2
                        let parsed = f.parse argv[index] argv[index + 1]
                        parse flags rest (result @ [ parsed ])
                    | None -> result, argv
            | [] -> result, argv

        parse flags argv []

type CommandBuilder (name : string, exe : CommandParseResult -> unit) =
    member b.name = name
    member val usage : string = "" with get, set
    member val parameters : IParam list = [] with get, set
    member val flags : IFlag list = [] with get, set
    member val optionalFlags : IFlag list = [] with get, set

    member b.build() =
        let parse (argv : string list) =
            let mutable argv = argv
            let mutable paramters : string list = []
            let mutable flags : FlagParseResult list = []

            if b.parameters.Length > 0 then
                CommandBuilder.parseParameters b.parameters argv
                |> fun (ps, rest) ->
                    paramters <- ps
                    argv <- rest

            if b.flags.Length > 0 then
                CommandBuilder.parseFlags b.flags argv
                |> fun (fs, rest) ->
                    flags <- fs
                    argv <- rest

            if b.optionalFlags.Length > 0 then
                CommandBuilder.parseFlags b.optionalFlags argv
                |> fun (fs, rest) ->
                    flags <- flags @ fs
                    argv <- rest

            { parameters = paramters; flags = flags }

        { usage = b.usage
          parse = parse
          execute = exe }
