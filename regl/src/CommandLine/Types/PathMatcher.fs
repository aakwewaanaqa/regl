namespace Regl.CommandLine.Types

type MatchResult =
    | Include
    | Exclude

type Pattern(value : string) =
    
    let isExcluding =
        value.Trim().StartsWith("!") |> not
        
    let value =
        let out = value.Trim()
        if out.StartsWith("!") then
            out.Substring(1)
        else
            out
    
    let fnMatch (src : string) (pattern : string) =
        let rec loopMatch (srcIndex : int) (patternIndex : int) =            
            if patternIndex = pattern.Length then
                true
            elif srcIndex = src.Length then
                false
            elif patternIndex > -1 then
                
                let isMatch =
                    let c = src[srcIndex]
                    let target = pattern[patternIndex]
                    match target with
                    | '*' -> true
                    | '?' -> true
                    | t -> c = t
                    
                let canSrcBackup =
                    pattern[patternIndex] = '*'
                
                if isMatch then
                    loopMatch (srcIndex + 1) (patternIndex + 1)
                elif canSrcBackup then
                    loopMatch (srcIndex - 1) patternIndex
                else
                    loopMatch srcIndex (patternIndex - 1)
            else
                false
        loopMatch 0 0
                    
    member p.tryMatch(path : string) =
        if fnMatch path value then
            if isExcluding then
                Some Exclude
            else
                Some Include
        else
            None

type PathMatcher() =
    member val patterns : Pattern list = [] with get, set
    
    member m.addPattern(value : string) =
        m.patterns <- m.patterns @ [ Pattern value ]
        
    member m.doMatch(path : string) =
        let 