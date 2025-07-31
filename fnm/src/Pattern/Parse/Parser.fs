namespace Fnm.Pattern.Parse

open Fnm.Helper

type Parser<'a> = StringCargo -> ('a * StringCargo) option
