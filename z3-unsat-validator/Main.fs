module Main

open validator

let success () = 0

let unSuccess () = 1

[<EntryPoint>]
let main =
  function
  | [| file; path |] -> success <| Validator.run file path
  | _ -> unSuccess <| printf "Invalid input: expected 2 arguments: file path"
