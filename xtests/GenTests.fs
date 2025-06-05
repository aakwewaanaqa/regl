module xtests.GenTests

open Xunit
open xtests.Shared

[<Fact>]
let ``test copy and output`` () =
    let result =
        doShellCmd "cat testSourceFile.txt | regl gen | regl to-file 'test copy and output.txt'"

    Assert.Equal(0, result.code)
