namespace PureTS.Parser

open System
open System.Numerics
open FParsec
open PureTS.Ast

exception ParserError of string

module Parsing =
    type Parser<'a> = Parser<'a, unit>

    let ws: Parser<unit> = spaces
    let ws1: Parser<unit> = spaces1
    let lexeme (p: Parser<'a>) = p .>> ws
    let str s = pstring s |> lexeme

    let identifier: Parser<string> =
        let isIdentStart c = isLower c || c = '_'
        let isIdentContinue c = isLetter c || isDigit c || c = '_'
        let raw = many1Satisfy2L isIdentStart isIdentContinue "identifier"
        attempt (
            raw >>= fun name ->
                match name with
                | "let" | "in" | "true" | "false" | "True" | "False" -> fail "reserved keyword"
                | _ -> preturn name
        )
        |> lexeme

    let numberLiteral: Parser<BigInteger> =
        many1SatisfyL isDigit "number"
        |>> BigInteger.Parse
        |> lexeme

    let boolLiteral: Parser<bool> =
        choice [
            stringReturn "true" true
            stringReturn "false" false
            stringReturn "True" true
            stringReturn "False" false
        ]
        |> lexeme

    let stringLiteral: Parser<string> =
        let chars = manyChars (noneOf "\"")
        between (pchar '\"') (pchar '\"') chars |> lexeme

    let expr, exprRef = createParserForwardedToRef<Expr, unit>()

    let parens p = between (str "(") (str ")") p

    let literal: Parser<Expr> =
        choice [
            numberLiteral |>> (fun n -> Expr.Literal (PureTS.Ast.Nat n))
            boolLiteral |>> (fun b -> Expr.Literal (PureTS.Ast.Bool b))
            stringLiteral |>> (fun s -> Expr.Literal (PureTS.Ast.String s))
        ]

    let recordField: Parser<string * Expr> =
        identifier .>>. (str "=" >>. expr)

    let recordExpr: Parser<Expr> =
        between (str "{") (str "}") (sepBy recordField (str ","))
        |>> Expr.Record

    let variable: Parser<Expr> =
        identifier |>> Expr.Var

    let primary: Parser<Expr> =
        choice [
            parens expr
            recordExpr
            literal
            variable
        ]

    let projSuffix: Parser<Expr -> Expr> =
        str "." >>. identifier |>> fun name -> fun f -> Expr.Proj(f, name)

    let postfix: Parser<Expr> =
        pipe2 primary (many projSuffix) (fun start suffixes ->
            List.fold (fun acc f -> f acc) start suffixes)

    let appExpr: Parser<Expr> =
        many1 postfix
        |>> fun terms ->
            terms |> List.reduce (fun acc arg -> Expr.App(acc, arg))

    let lambdaExpr: Parser<Expr> =
        let parameters = many1 identifier
        attempt (pchar '\\' >>. ws >>. parameters .>> ws .>> str "->" .>>. expr)
        |>> fun (names, body) -> Expr.Lambda(names, body)

    let letExpr: Parser<Expr> =
        str "let" >>. identifier .>> str "=" .>>. expr .>> str "in" .>>. expr
        |>> fun ((name, value), body) -> Expr.Let(name, value, body)

    do exprRef :=
        choice [
            attempt letExpr
            attempt lambdaExpr
            appExpr
        ]

    let program: Parser<Expr> = ws >>. expr .>> eof

module Lowering =
    let rec lower env expr =
        match expr with
        | Expr.Var name ->
            match env |> List.tryFindIndex (fun n -> n = name) with
            | Some idx -> PureTS.IR.Var idx
            | None -> PureTS.IR.Free name
        | Expr.Lambda (names, body) ->
            let rec build names env =
                match names with
                | [] -> lower env body
                | n :: rest -> PureTS.IR.Lam(build rest (n :: env))
            build names env
        | Expr.Let (name, value, body) ->
            PureTS.IR.App(PureTS.IR.Lam(lower (name :: env) body), lower env value)
        | Expr.App (f, x) ->
            PureTS.IR.App(lower env f, lower env x)
        | Expr.Record fields ->
            PureTS.IR.Record(fields |> List.map (fun (k, v) -> k, lower env v))
        | Expr.Proj (t, label) ->
            PureTS.IR.Proj(lower env t, label)
        | Expr.Literal lit ->
            match lit with
            | PureTS.Ast.Nat n -> PureTS.IR.Term.Literal (PureTS.IR.Nat n)
            | PureTS.Ast.Bool b -> PureTS.IR.Term.Literal (PureTS.IR.Bool b)
            | PureTS.Ast.String s -> PureTS.IR.Term.Literal (PureTS.IR.String s)

module API =
    let parse (input: string) =
        match run Parsing.program input with
        | Success (value, _, _) -> value
        | Failure (message, _, _) -> raise (ParserError message)

    let parseToIR (input: string) =
        parse input |> Lowering.lower []
