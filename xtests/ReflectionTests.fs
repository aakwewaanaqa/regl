namespace XTests

open System
open System.Reflection
open Regl.Program
open Xunit
open Xunit.Abstractions

module ReflectionTesting =

    type CommandCandidatesAttribute([<ParamArray>]types: Type array) =
        inherit Attribute()
        member this.candidates = types

    type Entry(
        [<CommandCandidates(
            typeof<Copy>,
            typeof<Split>)>]
        cmd: ICommand) =
        member this.cmd = cmd

    and ICommand =
        interface end

    and Copy() =
        interface ICommand

    and Split(delimiter: string) =
        interface ICommand
        member this.delimiter = delimiter

    let hasField (t: Type) field =
        t.GetRuntimeFields() |> Seq.tryFind (fun f -> f.Name = field) |> Option.isSome

    type ReflectionTests(output: ITestOutputHelper) =

        let log a = output.WriteLine $"{a}"

        [<Fact>]
        let ``Tests Concrete Type`` () =
            let t = typeof<Entry>

            t.GetConstructors()
            |> Array.iter (fun ctor ->
                           log ctor
                           ctor.GetParameters() |> Array.iter log)
            ()
