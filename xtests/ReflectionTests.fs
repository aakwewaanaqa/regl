namespace XTests

open System
open Regl.Program
open Xunit

module ReflectionTests =

    type Arguments() =
        member val cmd: Commands = Copy with get, set
        member val f: bool = false with get, set

    and Commands =
        | Copy
        | Split

    let argType = typeof<Arguments>

    let True b = Assert.True b

    let hasField field =
        let fieldInfo = argType.GetField field
        fieldInfo <> null

    [<Fact>]
    let ``Tests Union Type`` () =
        True(hasField "cmd")
        ()