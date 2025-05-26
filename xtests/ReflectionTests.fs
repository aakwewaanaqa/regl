namespace XTests

open System
open System.Reflection
open Regl.Program
open Xunit
open Xunit.Abstractions

module ReflectionTests =

    [<AttributeUsage(AttributeTargets.Property)>]
    type Optional() =
        inherit Attribute()

    type Arguments() =
        [<Optional>]
        member val cmd: Commands = Copy with get, set

        [<Optional>]
        member val f: bool = false with get, set

    and Commands =
        | Copy
        | Split

    let argType = typeof<Arguments>

    let True b = Assert.True b

    let flags = BindingFlags.NonPublic ||| BindingFlags.Public

    let hasField field =
        argType.GetRuntimeFields()
        |> Seq.tryFind(fun f -> f.Name = field)
        |> Option.isSome

    type Tests(output: ITestOutputHelper) =

        let log a = output.WriteLine $"{a}"

        [<Fact>]
        let ``Tests Union Type`` () =
            True(hasField "cmd@")
            True(hasField "f@")
            ()
