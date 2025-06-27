namespace Regl.CommandLine.Types.FlagsAndParams

open System
open System.Text.RegularExpressions

type ArgVal =
    | OfBool of bool
    | OfInt of int
    | OfString of string
    | OfRegex of Regex

    member v.valIs<'a> () =
        match v with
        | OfBool _ -> typeof<'a> = typeof<bool>
        | OfInt _ -> typeof<'a> = typeof<int>
        | OfString _ -> typeof<'a> = typeof<string>
        | OfRegex _ -> typeof<'a> = typeof<Regex>

    member v.value<'a> () =
        if v.valIs<'a> () then
            match v with
            | OfBool b -> Convert.ChangeType (b, typeof<'a>) :?> 'a
            | OfInt i -> Convert.ChangeType (i, typeof<'a>) :?> 'a
            | OfString s -> Convert.ChangeType (s, typeof<'a>) :?> 'a
            | OfRegex r -> Convert.ChangeType (r, typeof<'a>) :?> 'a
        else
            raise (InvalidCastException $"ArgVal({v}) can not have {typeof<'a>} as val")

    override v.ToString () =
        match v with
        | OfBool b -> b.ToString ()
        | OfInt i -> i.ToString()
        | OfString s -> s
        | OfRegex r -> r.ToString ()
