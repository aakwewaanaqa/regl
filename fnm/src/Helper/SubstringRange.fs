namespace Fnm.Helper

type SubstringRange =
    struct
        val startAt: int
        val endAt: int

        new(startAt, count) =
            { startAt = startAt
              endAt = startAt + count }

        new(startAt) = {
            startAt = startAt
            endAt = -1
        }
        
    end

module SubstringRange =
    let tryGet (src : string) (range : SubstringRange) =
        let isStartIn = range.startAt < src.Length
        let isEndIn = range.endAt < src.Length
        match range.endAt with
        | -1 when isStartIn ->
            src
            |> _.Substring(range.startAt)
            |> Some
        | endAt when isStartIn && isEndIn ->
            src
            |> _.Substring(range.startAt, endAt)
            |> Some
        | _ -> None