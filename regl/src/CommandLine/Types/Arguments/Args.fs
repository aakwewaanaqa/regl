namespace Regl.CommandLine.Types

open System
open System.Text
open System.Text.RegularExpressions
open Regl.Exts

type Args (rawArg : string) =
    let parse () : string list =
        let mutable result : string list = []
        let mutable quoting : char = ' '
        let mutable escaping : bool = false
        let builder = StringBuilder ()

        let isLongFlag () = builder.ToString().StartsWith ("--")
        let isInQuote () = quoting = ''' || quoting = '"'
        let isOfQuote (c : char) = quoting = c

        let rec loop (chars : char list) =
            match chars with
            | '\t' | ' ' as c :: rest ->
                if escaping || isInQuote () then
                    builder.Append c |> ignore
                    escaping <- false
                else if builder.Length > 0 then
                    result <- result @ [ builder.ToString () ]
                    builder.Clear () |> ignore

                loop rest
            | '=' as c :: rest ->
                if escaping then
                    builder.Append c |> ignore
                    escaping <- false
                elif isLongFlag () then
                    if isInQuote () then
                        builder.Append c |> ignore
                    else
                        result <- result @ [ builder.ToString () ]
                        builder.Clear () |> ignore
                else
                    builder.Append c |> ignore

                loop rest
            | ''' as c :: rest ->
                if escaping then
                    builder.Append c |> ignore
                    escaping <- false
                else if isOfQuote c then
                    quoting <- ' '
                elif isInQuote () then
                    builder.Append c |> ignore
                else
                    quoting <- c

                loop rest
            | '"' as c :: rest ->
                if escaping then
                    builder.Append c |> ignore
                    escaping <- false
                else if isOfQuote c then
                    quoting <- ' '
                elif isInQuote () then
                    builder.Append c |> ignore
                else
                    quoting <- c

                loop rest
            | '\\' as c :: rest ->
                if escaping then
                    builder.Append c |> ignore
                    escaping <- false
                else
                    escaping <- true

                loop rest
            | c :: rest ->
                builder.Append c |> ignore
                loop rest
            | [] ->
                if builder.Length > 0 then
                    result <- result @ [ builder.ToString () ]
                else
                    ()

        loop (rawArg.ToCharArray () |> List.ofArray)

        result

    member val args = parse () with get, set

    member this.length = this.args.Length

    member this.Item
        with get index = this.args[index]

    member this.Length = this.args.Length

    new (args : string list) as this =
        Args ""
        then this.args <- args

and ArgsDto = {
    flag : IFlag
    flagVal : FlagVal
    rem : Args
}

module Args =
    let isFlag (n : string) = n.StartsWith "-"

    let isShortFlag (n : string) =
        let shortFlagPattern = Regex "^-[a-zA-Z0-9]+"
        shortFlagPattern.IsMatch n

    let isLongFlag (n : string) =
        let longFlagPattern = Regex "^--[a-zA-Z\-0-9]+"
        longFlagPattern.IsMatch n

    let hasFlag (flag : IFlag) (args : Args) =
        try
            let args = args.args
            let argShortFlags = args |> List.filter isShortFlag
            guard (args.Length <= 0) "args was not provided..."

            if flag.name |> isShortFlag then
                guard (argShortFlags |> List.isEmpty) $"provided args({args}) has not short flags"
                let flat = argShortFlags |> List.reduce (fun a b -> $"{a}{b}")

                for a in flag.name do
                    flat
                    |> Seq.tryFindIndex (fun c -> c.Equals a)
                    |> function
                        | None -> guard true $"args({args}) lacks -{a}"
                        | Some _ -> ()
            elif flag.name |> isLongFlag then
                guard
                    (args |> List.exists (fun arg -> arg.Equals flag.name) |> not)
                    $"flag({flag}) is not in args({args})"
            else
                guard true $"flag({flag}) is not a long flag or short flag neither"

            Ok ()
        with ex ->
            Error ex

    let getValue (flag : IFlag) (args : Args) =
        try
            let args = args.args
            // it could be a short flag also
            // guard (should.name |> isLongFlag |> not) $"flag({should}) should be a long flag"
            let inIndex = args |> List.tryFindIndex (fun arg -> arg.Equals flag.name)
            guard inIndex.IsNone $"flag({flag}) was not in the args({args})"
            let inIndex = inIndex.Value
            guard (inIndex + 1 >= args.Length) $"args({args}) at index {inIndex + 1} should have val"
            guard (args[inIndex + 1] |> isFlag) $"args({args}) at index {inIndex + 1} was a flag"
            if flag.needInput then
                let flagVal = flag.getVal args[inIndex + 1]
                let rem = args |> List.removeManyAt inIndex 2 |> Args
                Ok { flag = flag; flagVal = flagVal; rem = rem }
            else
                let flagVal = flag.getVal ""
                let rem = args |> List.removeAt inIndex |> Args
                Ok { flag = flag; flagVal = flagVal; rem = rem }
        with ex ->
            Error ex