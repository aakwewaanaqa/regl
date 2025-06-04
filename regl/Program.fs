namespace Regl

module Program =
    /// Entry point for the application
    /// Returns 0 to indicate successful execution
    [<EntryPoint>]
    let main argv =
        0
        //
        // try
        //     let matchedCommand = 
        //         cmds
        //         |> List.tryFind (fun cmd -> cmd.parse argv |> Option.isSome)
        //
        //     match matchedCommand with
        //     | Some cmd ->
        //         cmd.execute (cmd.parse argv)
        //         0
        //     | None ->
        //         printfn "Available commands:"
        //         cmds
        //         |> List.choose _.usage
        //         |> List.iter (printfn "%s")
        //         1
        // with ex ->
        //     printfn $"Error: {ex}"
        //     1