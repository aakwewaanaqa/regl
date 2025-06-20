namespace Regl.CommandLine.Types

open System
open Microsoft.FSharp.Core.LanguagePrimitives

type FlagParseResult = {
    name : string
    value : FlagVal
}

and FlagVal =
    | OfBool of bool
    | OfText of string

    override f.ToString() : string =
        match f with
        | OfBool b -> $"{b}"
        | OfText str -> str

module FlagVal =
    let defaultString (a : string) (opt : FlagVal option) =
        if opt.IsSome then
            match opt.Value with
            | OfText str -> str
            | OfBool b -> $"{b}"
        else
            a