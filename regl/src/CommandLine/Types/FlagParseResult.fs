namespace Regl.CommandLine.Types

type FlagParseResult = {
    name : string
    value : FlagParseValue
}

and FlagParseValue =
    | OfBool of bool
    | OfText of string