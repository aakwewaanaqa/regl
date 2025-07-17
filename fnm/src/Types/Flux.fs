namespace fnm.Types

type Flux =
    struct
        val private _src: string
        val private _index: int

        new(src: string, index: int) = { _src = src; _index = index }

        member f.head: char = f._src[f._index]

        member f.src: string = f._src
        
        member f.index: int = f._index
    end

module Flux =
    let take (count: int) (f: Flux) = Flux(f.src, f.index + count)
