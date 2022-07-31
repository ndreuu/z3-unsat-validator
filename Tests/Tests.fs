module Tests

open System
open System.IO
open NUnit.Framework

let dir =
  sprintf "%s/samples%s" Environment.CurrentDirectory

let readLines (filePath: string) =
  seq {
    use sr = new StreamReader(filePath)

    while not sr.EndOfStream do
      yield sr.ReadLine()
  }

let eq =
  fun x y -> Seq.fold (&&) true (Seq.zip x y |> Seq.map (fun (x', y') -> x' = y'))

let compFolders =
  fun fst snd ->
    Directory.GetFiles(dir fst, "*.smt2")
    |> fun x ->
         Directory.GetFiles(dir snd, "*.smt2")
         |> fun y -> Array.fold2 (fun acc x' y' -> acc && eq (readLines x') (readLines y')) true x y


[<SetUp>]
let Setup () = ()

[<Test>]
let Test1 () =
  Assert.IsTrue(compFolders "/drop_id" "/drop_id-match")
