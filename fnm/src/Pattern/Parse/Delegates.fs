namespace Fnm.Pattern.Parse

open Fnm.Helper

type Matcher =
    | CannotFail of (StringCargo -> StringCargo option)
    | CanRetry of (StringCargo -> StringCargo option)
    
    member m.func =
        match m with
        | CannotFail f -> f
        | CanRetry f -> f

type Parser = StringCargo -> (Matcher * StringCargo) option
