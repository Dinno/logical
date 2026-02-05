namespace PureTS.Ast

open System.Numerics

type Literal =
    | Nat of BigInteger
    | Bool of bool
    | String of string

type Expr =
    | Var of string
    | Lambda of string list * Expr
    | Let of string * Expr * Expr
    | App of Expr * Expr
    | Record of (string * Expr) list
    | Proj of Expr * string
    | Literal of Literal
