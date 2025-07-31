namespace Fnm.Pattern.Parse

open Fnm.Helper

type Matcher = StringCargo -> StringCargo option

type Parser = StringCargo -> (Matcher * StringCargo) option
