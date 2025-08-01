namespace Fnm.Helper

type CharCargo =
    | Escaped of char
    | NotEscaped of char
    
    member c.character =
        match c with
        | Escaped c -> c
        | NotEscaped c -> c