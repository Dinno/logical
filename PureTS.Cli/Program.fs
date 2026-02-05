open System
open PureTS.Parser
open PureTS.IR

let readInput (args: string array) =
    if args.Length > 0 then
        String.Join(" ", args)
    else
        Console.In.ReadToEnd()

[<EntryPoint>]
let main args =
    try
        let input = readInput args
        let term = API.parseToIR input
        printfn "%s" (Pretty.toString term)
        0
    with
    | ParserError message ->
        eprintfn "%s" message
        1
