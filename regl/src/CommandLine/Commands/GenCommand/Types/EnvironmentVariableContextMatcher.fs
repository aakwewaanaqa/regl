namespace Regl.CommandLine.Commands.GenCommand.Types

open System
open System.Text.RegularExpressions
open Regl.CommandLine.Shared

type EnvironmentVariableContextMatcher(pattern: Regex, format: string, envarName: string) =
    member this.doMatch(ctx: string) =
        pattern.Matches ctx
        |> Seq.iter (fun m -> Environment.SetEnvironmentVariable(envarName, formatMatch m format))
