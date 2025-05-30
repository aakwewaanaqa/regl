namespace XTests

open System
open System.Reflection
open System.Runtime.InteropServices
open Regl.Program
open Xunit
open Xunit.Abstractions

module ReflectionTesting =

    let cmdParser (ctor: ConstructorInfo) (argv: string array) =
        /// if argv is the name of the cmd,
        /// and we ignore the case here
        let isCmdName (name) =
            argv.Length > 0 &&
                String.Equals(argv[0], name, StringComparison.OrdinalIgnoreCase)

        /// if argv have the enough counts for the parser
        let isEnoughArgs (count) =
            argv.Length >= count + 1 // 'cause the first is the name of cmd

        let tryParseArgs (prmtrs: ParameterInfo array) =
            seq {
                for i in 0 .. prmtrs.Length - 1 do 
                    match prmtrs[i].ParameterType with
                    | t when t = typeof<int> -> yield argv[i+1] |> int |> box
                    | t when t = typeof<float> -> yield argv[i+1] |> float |> box
                    | t when t = typeof<string> -> yield argv[i+1] |> box 
                    | t -> raise (NotSupportedException($"Type {t.Name} is not supported"))
            } |> Array.ofSeq

        try
            let prmtrs = ctor.GetParameters()
            if not (isCmdName ctor.DeclaringType.Name) then None
            else if not (isEnoughArgs prmtrs.Length) then None
            else
                let args = tryParseArgs prmtrs
                Some(ctor.Invoke args)
        with _ ->
            None
    
    type CommandCandidatesAttribute([<ParamArray>]types: Type array) =
        inherit Attribute()
        member this.candidates = types
        member this.parsers =
            seq {
                for one in this.candidates do
                    let ctor = one.GetConstructors() |> Array.head
                    let fn   = cmdParser ctor
                    yield fn
            } |> Array.ofSeq

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

            let parsers = t.GetConstructors()
                          |> Array.head
                          |> _.GetParameters()
                          |> Array.head
                          |> _.GetCustomAttribute<CommandCandidatesAttribute>()
                          |> _.parsers

            Assert.True(parsers.Length = 2)
            
            ()
