namespace Fnm.Types

type PatternCargo =
    struct
        val private _isIn: bool
        val private _path: string

        new(isIn, path) = { _isIn = isIn; _path = path }

        member pc.isIn = pc._isIn
        member pc.path = pc._path
        member pc.exclude() = PatternCargo(false, pc._path)
        member pc.include() = PatternCargo(true, pc._path)
    end
