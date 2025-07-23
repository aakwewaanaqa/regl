namespace Fnm.Types

open System
open System.IO

type Matcher =
    struct
        val private _patterns : Pattern list 
        
        new (patterns : Pattern list) = { _patterns = patterns }
                
        member m.visit (path : string) =
            let rec loopVisit (patterns : Pattern list) (cargo : PatternCargo) =
                match patterns with
                | head :: tail ->
                    cargo
                    |> head.visit
                    |> loopVisit tail
                | [] ->
                    cargo
            PatternCargo(true, path)
            |> loopVisit m._patterns
    end