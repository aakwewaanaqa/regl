namespace regl

open System

module Program =
    /// Entry point for the application
    /// Returns 0 to indicate successful execution
    [<EntryPoint>]
    let main argv =
        let commands = [ Commands.copyCmd; Commands.splitCmd; Commands.matchCmd ]

        try
            commands
            |> List.tryPick (fun cmd -> cmd.parse argv)
            |> Option.iter (fun result ->
                commands
                |> List.tryFind (fun cmd -> cmd.parse argv |> Option.isSome)
                |> Option.iter (fun cmd -> cmd.execute (cmd.parse argv)))

            Console.WriteLine Commands.pIn

            0
        with ex ->
            printfn $"Error: {ex}"
            1
