namespace Regl

open System

module Program =

    type IFlag =
        abstract member name: string

    type IInFlag<'a> =
        inherit IFlag
        abstract member value: 'a
        abstract member tryParse: string -> string -> IInFlag<'a> option

    and OnFlag(name) =
        interface IFlag with
            member f.name = name

    and InString(name, value) =
        interface IInFlag<string> with
            member f.name = name
            member f.value: string = value

            member f.tryParse arg1 arg2 =
                if arg1 <> arg1 then None else Some(InString(arg1, arg2))

        new(name) = InString(name, "")

    /// Represents a builder for creating command line argument parsers
    /// with support for required parameters, required arguments and optional arguments
    type CommandBuilder(name: string) =
        /// the name of the command
        member this.name = name
        /// the usage of the command
        member val usage = None with get, set
        /// mean to be in the first of all flags and by order
        member val requiredParamsCount = 0 with get, set
        /// mean to be after params and in any order but required
        member val requiredFlags = List<IFlag>.Empty with get, set
        /// mean to be after params and in any order but can be optionally given
        member val optionalFlags = List<IFlag>.Empty with get, set

        /// Makes tryParser to return Some or None
        member private this.tryParse(argv: string array) =
            /// Determines if the given argument is a flag by checking if it starts with - or --
            let isFlag (arg: string) =
                arg.StartsWith("-") || arg.StartsWith("--")

            /// Gets the value associated with a flag at the given index, if one exists
            let getFlagValue (argv: string array) (index: int) =
                if index + 1 < argv.Length && not (isFlag argv[index + 1]) then
                    Some argv[index + 1]
                else
                    None

            // Attempts to parse the command line arguments according to the command's requirements
            if argv.Length < (1 + this.requiredParamsCount + this.requiredFlags.Length) then
                None
            else if argv[0] <> name then
                None
            else if
                not (
                    this.requiredFlags
                    |> List.forall (fun f -> argv |> Array.exists (fun arg -> arg = f.name))
                )
            then
                None
            else
                let rem = argv[1..]
                let prmtrs = rem[.. this.requiredParamsCount - 1]

                let parseFlags (args: string array) =
                    args
                    |> Array.mapi (fun i arg ->
                        if isFlag arg then
                            match getFlagValue args i with
                            | Some value ->
                                match
                                    this.requiredFlags @ this.optionalFlags |> List.tryFind (fun f -> f.name = arg)
                                with
                                | Some flag ->
                                    match flag with
                                    | :? IInFlag<'a> as inFlag ->
                                        match inFlag.tryParse arg value with
                                        | Some parsedFlag -> Some(parsedFlag :> IFlag)
                                        | None -> Some(OnFlag(arg))
                                    | _ -> Some(OnFlag(arg))
                                | None -> Some(OnFlag(arg))
                            | None -> Some(OnFlag(arg))
                        else
                            None)
                    |> Array.choose id

                let flags = rem[this.requiredParamsCount ..] |> parseFlags

                let unused =
                    rem[this.requiredParamsCount ..] |> Array.filter (fun arg -> not (isFlag arg))

                Some(
                    { prmtrs = prmtrs
                      args = flags
                      rem = unused }
                )

        member this.build() = this.tryParse

    /// Represents the result of parsing command line arguments
    /// containing both positional parameters and named arguments
    and ParseResult =
        { prmtrs: string array // Array of positional parameters
          args: IFlag array // Array of named flags with their values
          rem: string array } // Array of remaining of argv

    /// Entry point for the application
    /// Returns 0 to indicate successful execution
    [<EntryPoint>]
    let main argv =

        let pIn = Console.In.ReadToEnd()

        let copyCmd = CommandBuilder("copy").build ()

        let splitCmd =
            let splitBuilder = CommandBuilder("split")
            splitBuilder.requiredParamsCount <- 1
            splitBuilder.build ()

        let matchCmd =
            let matchBuilder = CommandBuilder("match")
            matchBuilder.requiredParamsCount <- 1
            matchBuilder.optionalFlags <- [ InString("--format") ]
            matchBuilder.build ()



        0
