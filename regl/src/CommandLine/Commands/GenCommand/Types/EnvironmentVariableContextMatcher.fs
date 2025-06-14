namespace Regl.CommandLine.Commands.GenCommand.Types

open System.Text.RegularExpressions
open Regl.CommandLine.IO.InOut

type EnvironmentVariableContextMatcher(pattern: Regex, format: string, envarName: string) =
    let formatMatch (m: Match) (format: string) =
        let mutable formatted = format
        m.Groups
        |> Seq.iteri (fun i g ->
            if g.Success then
                formatted <- formatted.Replace($"${i}", g.Value)
            )
        formatted

    member this.doMatch(ctx: string) =
        let matches = pattern.Matches ctx
        for m in matches do
            if m.Success then
                setEnvar envarName (formatMatch m format)
