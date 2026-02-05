namespace PureTS.VM

open PureTS.IR

type Value =
    | VClosure of Env * Term
    | VRecord of Map<string, Value>
    | VLiteral of Literal
    | VFree of string

and Env = Value list

exception RuntimeError of string

module Eval =
    let rec private eval (env: Env) (term: Term) : Value =
        match term with
        | Term.Var index ->
            match env |> List.tryItem index with
            | Some value -> value
            | None -> raise (RuntimeError $"Unbound variable index {index}")
        | Term.Lam body ->
            VClosure(env, body)
        | Term.App (fn, arg) ->
            let fnVal = eval env fn
            let argVal = eval env arg
            apply fnVal argVal
        | Term.Record fields ->
            fields
            |> List.map (fun (k, v) -> k, eval env v)
            |> Map.ofList
            |> VRecord
        | Term.Proj (target, label) ->
            match eval env target with
            | VRecord fields ->
                match fields.TryFind(label) with
                | Some value -> value
                | None -> raise (RuntimeError $"Missing field '{label}'")
            | _ ->
                raise (RuntimeError "Projection on non-record value")
        | Term.Free name ->
            VFree name
        | Term.Literal lit ->
            VLiteral lit

    and private apply fnVal argVal =
        match fnVal with
        | VClosure (env, body) ->
            eval (argVal :: env) body
        | VFree name ->
            raise (RuntimeError $"Cannot apply free symbol '{name}'")
        | _ ->
            raise (RuntimeError "Attempted to apply non-function value")

    let run (term: Term) =
        eval [] term

module Pretty =
    let rec valueToString value =
        match value with
        | VClosure _ -> "<closure>"
        | VRecord fields ->
            let items =
                fields
                |> Seq.map (fun kv -> $"{kv.Key}: {valueToString kv.Value}")
                |> String.concat ", "
            $"{{{items}}}"
        | VLiteral lit ->
            match lit with
            | Literal.Nat n -> n.ToString()
            | Literal.Bool true -> "true"
            | Literal.Bool false -> "false"
            | Literal.String s -> $"\"{s}\""
        | VFree name -> name
