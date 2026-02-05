namespace PureTS.IR

open System.Numerics

type Literal =
    | Nat of BigInteger
    | Bool of bool
    | String of string

type Term =
    | Var of int
    | Lam of Term
    | App of Term * Term
    | Record of (string * Term) list
    | Proj of Term * string
    | Free of string
    | Literal of Literal

module Encodings =
    let zero = Free "zero"
    let succ = Free "succ"

    let rec peano (n: int) =
        if n <= 0 then zero
        else App(succ, peano (n - 1))

    let churchTrue = Lam(Lam(Var 1))
    let churchFalse = Lam(Lam(Var 0))

module Pretty =
    let rec toString term =
        match term with
        | Var i -> $"v{i}"
        | Lam body -> $"(λ {toString body})"
        | App (f, x) -> $"({toString f} {toString x})"
        | Record fields ->
            let items =
                fields
                |> List.map (fun (k, v) -> $"{k}: {toString v}")
                |> String.concat ", "
            $"{{{items}}}"
        | Proj (t, label) -> $"{toString t}.{label}"
        | Free name -> name
        | Literal lit ->
            match lit with
            | Nat n -> n.ToString()
            | Bool true -> "true"
            | Bool false -> "false"
            | String s -> $"\"{s}\""
