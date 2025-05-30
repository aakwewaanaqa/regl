namespace Regl

module Program =
    /// Represents a builder for creating command line argument parsers
    /// with support for required parameters, required arguments and optional arguments
    type CommandBuilder(name: string) =
        /// the name of the command
        member this.name = name
        /// the usage of the command
        member val usage = None with get, set
        /// mean to be in the first of all args and in order
        member val requiredParamsCount = 0 with get, set
        /// mean to be after params and in any order but required
        member val requiredArgs = List<string>.Empty with get, set
        /// mean to be after params and in any order but can be optionally given
        member val optionalArgs = List<string>.Empty with get, set

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
            if argv.Length < (1 + this.requiredParamsCount + this.requiredArgs.Length) then
                None
            else if argv[0] <> name then
                None
            else if
                not (
                    this.requiredArgs
                    |> List.forall (fun name -> argv |> Array.exists (fun arg -> arg = name))
                )
            then
                None
            else
                let rem = argv[1..]
                let prmtrs = rem[.. this.requiredParamsCount - 1]

                let flags =
                    rem[this.requiredParamsCount ..]
                    |> Array.mapi (fun i arg ->
                        if isFlag arg then
                            (arg, getFlagValue rem[this.requiredParamsCount ..] i)
                        else
                            (arg, None))
                    |> Array.filter (fun (arg, _) -> isFlag arg)

                Some({ prmtrs = prmtrs; args = flags })

        member this.build() = this.tryParse

    /// Represents the result of parsing command line arguments
    /// containing both positional parameters and named arguments
    and ParseResult =
        { prmtrs: string array  // Array of positional parameters
          args: (string * string option) array }  // Array of named arguments with their values

    /// Entry point for the application
    /// Returns 0 to indicate successful execution
    [<EntryPoint>]
    let Main args = 0
