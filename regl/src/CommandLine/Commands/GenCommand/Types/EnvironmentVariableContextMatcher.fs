namespace Regl.CommandLine.Commands.GenCommand.Types

open System
open System.Text.RegularExpressions

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
        pattern.Matches ctx
        |> Seq.iter (fun m -> Environment.SetEnvironmentVariable(envarName, formatMatch m format))
