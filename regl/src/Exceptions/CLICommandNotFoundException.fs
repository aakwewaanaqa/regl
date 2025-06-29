namespace Regl.Exceptions

open System

type CLICommandNotFoundException(cmdName) =
    inherit exn($"No command called {cmdName} for regl")