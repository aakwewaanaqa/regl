namespace Regl.Exceptions

open System

type CLIEntryNotValidException(cmdName) =
    inherit exn()
    
    member ex.cmdName : string = cmdName

    override ex.Equals (obj : obj) =
        ex.GetHashCode() = obj.GetHashCode()
        
    override ex.GetHashCode () : int =
        HashCode.Combine(ex.cmdName)
