//these are similar to C# using statements
open canopy
open runner
open System
open Microsoft.FSharp.Data
open System.IO

let inline (|??) (a: 'a Nullable) b = if a.HasValue then a.Value else b

start chrome

"Load julies comments" &&& fun _ ->
    url "http://msdn.microsoft.com/sv-se/magazine/ee532098(en-us).aspx?sdmr=JulieLerman&sdmi=authors"

    let links = elements ".ResultTitleLink"
    let urls = links |> List.map(fun(l) -> l.GetAttribute("href"))

    let articles = urls |> List.map (fun u -> 
        url u
        let title = (((element ".FeatureTitle") |> elementsWithin "h1,h2") |> List.rev).Head.Text
        let commentCount = (unreliableElements ".Clone").Length
        let lastComment = if commentCount > 0 then ((first ".Clone") |> elementWithin ".bodyComments" |> elementsWithin "span").Head.Text |> DateTime.Parse else new DateTime()
        (commentCount, lastComment , title, u)
    )

    let lines = articles |> Seq.sortBy (fun (commentCount, lastComment , title, u) -> lastComment) |> Seq.map (fun(commentCount, lastComment , title, u) -> String.Format("{0}\t\t{1}\t\t{2}\t\t{3}", commentCount, lastComment , title, u))

    File.WriteAllLines("result.txt", Seq.toArray lines)
//    if not (File.Exists("result.csv"))
//    then File.Create("result.csv") |> ignore
//
//    let oldLines = File.ReadAllLines("result.csv") |> Array.map(fun row -> row.Split(';'))
//
//    articles
//
//    File.WriteAllLines("result.csv", Seq.toArray lines)
//
//    printfn "%O" articles

//run all tests
run()

System.Console.WriteLine("press [enter] to exit")
System.Console.ReadLine() |> ignore

quit()