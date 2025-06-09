namespace Regl.CommandLine.Types

type FlagParseResult = {
    name : string
    value : FlagOption
}

and FlagOption =
    | OfBool of bool
    | OfText of string

    override f.ToString() : string =
        match f with
        | OfBool b -> $"{b}"
        | OfText str -> str

module FlagOption =
    let defaultString (a : string) (opt : FlagOption option) =
        if opt.IsSome then
            match opt.Value with
            | OfText str -> str
            | OfBool b -> $"{b}"
        else
            a