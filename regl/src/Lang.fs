module Regl.Lang

open System
        
module ExceptionLang =
    let parametersNotEnough : Exception =
        Exception("The parameters was not enough to be parsed...")
    let bashCrash (code : int) : Exception =
        SystemException($"/bin/bash crashed... crash code is {code}")