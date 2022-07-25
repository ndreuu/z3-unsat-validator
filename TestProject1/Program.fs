open System
open NUnit.Framework
open SMTLIB2
open SMTLIB2.Parser
open System.IO
open System.Text.RegularExpressions

module Program =
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

    let addDeclarations =
        fun fileFrom fileTo mode ->
            let decls =
                Parser().ParseFile(fileFrom)
                |> declarations
                |> List.map (fun x -> x.ToString())

            let content = File.ReadAllText(fileTo)

            let content', _ =
                let HEADER = "(set-logic HORN)"
                let ENDL_SIZE = 1

                let position =
                    let regEx = Regex(HEADER).Match(HEADER)

                    match regEx.Success with
                    | true -> Some <| regEx.Index + HEADER.Length
                    | _ -> None

                match position with
                | Some pos ->
                    (List.fold
                        (fun (acc: string, pdng) x ->
                            sprintf "%s%s\n%s" acc.[0 .. pdng - 1] x acc.[pdng..], pdng + x.Length + ENDL_SIZE)
                        (content, pos + ENDL_SIZE))
                        decls
                | None -> (content, 0)

            let content' = (content', "_\d+", "") |> mode

            if content <> content' then
                File.WriteAllText(fileTo, content')


    let asserts =
        let isProof =
            function
            | Command (Proof _) -> true
            | _ -> false

        List.filter isProof
        >> List.head
        >> function
            | Command (Proof (h, Asserted a, s)) ->
                let conclusion =
                    function
                    | HyperProof (_, _, s) -> Assert s

                let task =
                    function
                    | HyperProof (Asserted a, ps, s) ->
                        let conclusions = ps |> List.map conclusion
                        (Assert a) :: conclusions @ [ Assert <| Not s ]

                let rec collect =
                    function
                    | (HyperProof (_, hps, _) as hypProof) ->
                        let cur = task hypProof

                        let childs hps =
                            let rec helper acc =
                                function
                                | HyperProof (_, hps, _) as hypProof :: hs -> helper (task hypProof :: acc) (hps @ hs)
                                | [] -> acc

                            helper [ cur ] hps

                        childs hps

                let modesPones = [ Assert <| Not a; conclusion h ]

                modesPones :: collect h |> List.rev |> Some
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
                    let a = List.fold (fun acc x -> sprintf "%s%s\n\n" acc <| x.ToString()) "" x
                    (sprintf "%s" a) :: acc)
                []
                cmds
            |> List.rev
        | _ -> []

    let write =
        fun path tasks ->
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
                    |> List.map (fun x -> "___" + x.ToString())
                    |> List.rev
                    |> function
                        | _ :: xs -> "___mp" :: xs |> List.rev |> Some
                        | _ -> None

                | _ -> None

            match names tasks with
            | Some names ->
                let path name = path + "/" + name

                List.fold2 (fun _ name content -> File.WriteAllText(path name, content)) () names (tasksToStrs tasks)
            | _ -> ()

[<EntryPoint>]
let main _ =
    let file = "/home/andrew/smt/analysis-vampire/rin-gen/chc-LIA_060.smt2"

    let parser = Parser()

    let cmds = parser.ParseFile <| file

    match Program.tasks cmds with
    | Some cmds ->
        List.fold
            (fun _ x ->
                printfn "\n\n\n"
                List.fold (fun _ x -> printfn "%s\n" <| x.ToString()) () x)
            ()
            cmds
    | _ -> ()

    let path = "/home/andrew/smt/analysis-vampire/rin-gen"

    Program.tasks cmds |> Program.write path

    0
