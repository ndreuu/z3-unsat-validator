module Main

open validator

[<EntryPoint>]
let main =
    function
    | [| file; path |] ->
        let () = Validator.run file path
        0
    | _ ->
        let () = printf "expected 2 arguments: file path"

        1

