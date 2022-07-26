namespace validator

module Validator =
  open SMTLIB2
  open SMTLIB2.Parser
  open System.IO

  let declarations =
    let isDecl =
      function
      | Command (DeclareDatatype _)
      | Command (DeclareDatatypes _)
      | Command (DeclareFun _)
      | Command (DeclareSort _)
      | Command (DeclareConst _) -> true
      | _ -> false

    List.filter isDecl


  let asserts =
    let isProof =
      function
      | Command (Proof _) -> true
      | _ -> false

    List.filter isProof
    >> List.head
    >> function
      | Command (Proof (h, Asserted a, _)) ->
        let conclusion (HyperProof (_, _, s)) = Assert s

        let task (HyperProof (Asserted a, ps, s)) =
          let conclusions = ps |> List.map conclusion
          (Assert a) :: conclusions @ [ Assert <| Not s ]

        let rec collect (HyperProof (_, hps, _) as hypProof) =
          let cur = task hypProof

          let childs hps =
            let rec helper acc =
              function
              | HyperProof (_, hps, _) as hypProof :: hs -> helper (task hypProof :: acc) (hps @ hs)
              | [] -> acc

            helper [ cur ] hps

          childs hps

        let modesPones =
          [ Assert <| Not a; conclusion h ]

        //        modesPones ::
        collect h |> List.rev |> Some
      | _ -> None

  let tasks =
    fun cmds ->
      match asserts cmds with
      | Some asserts ->
        Some
        <| List.map (fun x -> declarations cmds @ x @ [ Command CheckSat ]) asserts
      | _ -> None

  let tasksToStrs =
    function
    | Some cmds ->
      List.fold
        (fun acc x ->
          let a =
            List.fold (fun acc x -> sprintf "%s%s\n\n" acc <| x.ToString()) "" x

          (sprintf ";(set-logic UFDT)\n\n%s" a) :: acc)
        []
        cmds
    //      |> function
//        | _ :: xs -> xs |> List.rev
//        | _ -> []
    | _ -> []

  let write =
    fun path fName tasks ->
      let names =
        function
        | Some (cmds: 'a list) ->
          let _, res =
            List.fold
              (fun (i, acc) _ ->
                if i > 0 then
                  (i - 1, i :: acc)
                else
                  (i, acc))
              (cmds.Length, [])
              cmds

          res
          |> List.map (fun x -> sprintf "___%s.smt2" <| x.ToString())
          |> List.rev
          |> Some
        //          |> function
//            | _ :: xs -> xs |> List.rev |> Some
//            | _ -> None

        | _ -> None

      match names tasks with
      | Some names ->
        let path name = sprintf "%s/%s" path name

        List.fold2 (fun _ name content -> File.WriteAllText(path name, content)) () names
        <| tasksToStrs tasks
      | _ -> ()

  let public run =
    fun (file: string) path ->
      let write =
        file
        |> Path.GetFileNameWithoutExtension
        |> write path

      file |> Parser().ParseFile |> tasks |> write
