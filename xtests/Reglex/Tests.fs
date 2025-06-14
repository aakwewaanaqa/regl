module XTests.Reglex.Tests

open System.Text.RegularExpressions
open Xunit
open Xunit.Abstractions

type ParseGroup = {
    rem : string
    value : string
}

type Tests (helper : ITestOutputHelper) =
    [<Fact>]
    let ``test parse recursively`` () =
        let pChar (c : char) (g : ParseGroup) =
            if g.rem[0].Equals c then
                Some { rem = g.rem[1..]; value = g.value + c.ToString() }
            else
                None
        let pId (g : ParseGroup) =
            let m = Regex("^[a-zA-Z][_0-9a-zA-Z]*").Match(g.rem)
            if m.Success then
                Some { rem = g.rem[m.Length..]; value = g.value + m.Value }
            else
                None
        let rec pGenericId (src : ParseGroup) : ParseGroup option =
            let g = pId src
            match g with
            | Some g ->
                match pChar '<' g with
                | Some g ->
                    match pGenericId g with
                    | Some g -> pChar '>' g
                    | None -> None
                | None -> Some g
            | None -> None

        let g = pGenericId { rem = "Task<Response>"; value = "" }
        g.IsSome |> Assert.True
        (g.Value.value, "Task<Response>") |> Assert.Equal
        
        let g = pGenericId { rem = "Task<Response>>"; value = "" }
        g.IsSome |> Assert.True
        (g.Value.value, "Task<Response>") |> Assert.Equal