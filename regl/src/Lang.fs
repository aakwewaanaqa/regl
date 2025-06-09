module Regl.Lang

open System

let lineCountExplain = "The count of line for reading"

let regexMatcherExplain = "A Regex to match the whole CONTEXT"

let matchOutputFormatExplain = "A format to write as a match, uses $1 $2 $3 ... as groups"

let envarNameExplain = "The target environment variable"

let addEvcmUsage = $"add-evcm <REGEX> <MATCH-OUTPUT-FORMAT> <ENVAR-NAME>
    Adds an environmental variable matcher for
    tpl command when reading the source file's line(s) as its `CONTEXT`
        <REGEX>               : {regexMatcherExplain}
        <MATCH-OUTPUT-FORMAT> : {matchOutputFormatExplain}
        <ENVAR-NAME>          : {envarNameExplain}
"

let templateBashFileExplanation = "The file to be executed with environment variable"

let tplUsage = $"tpl <LINE-COUNT> <TEMPLATE-BASH-FILE>
    Reads lines of <LINE-COUNT> in source file as `CONTEXT`
    and executes added Evcm with `CONTEXT`
    and then executes <TEMPLATE-BASH-FILE>
    and finally outputs the echoed messages to `OUT`
        <LINE-COUNT>         : {lineCountExplain}
        <TEMPLATE-BASH-FILE> : {templateBashFileExplanation}
"

module CommandLang =
    let splitUsage = "regl split <delimiter>
    
    "

module ExceptionLang =
    let parametersNotEnough : Exception =
        Exception("The parameters was not enough to be parsed...")
    let bashCrash (code : int) : Exception =
        SystemException($"/bin/bash crashed... crash code is {code}")