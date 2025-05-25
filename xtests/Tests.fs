namespace XTests

module Testing =

    open System
    open System.IO
    open Xunit
    open Xunit.Abstractions

    let True b = Assert.True b

    type Tests(helper: ITestOutputHelper) =
        [<Fact>]
        let ``Test Copy`` () =
            let pipeIn = "Hello World!"
            use reader = new StringReader(pipeIn)
            Console.SetIn reader

            let rt = Regl.Program.Main([| "copy" |])
            let isPassed = rt = 0
            True(isPassed)
