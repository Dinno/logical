{{
import { createInteger, createTypeAttr } from "../formalBuilders";
import { Kind, Application, Abstraction, Variable } from "../formalTree";
}}

start 
    = _ expr:expr _ { return expr; }

expr "expression"
    = abstraction / integer / var

abstraction "function definition"
    = name: name _ ":" _ type: (kind / expr) _ ("=>" / ";") _ body: expr { return new Abstraction(name, body, [createTypeAttr(type)]) }
    = name: name _ ("=>" / ";") _ body: expr {return new Abstraction(name, body, [])}

application "function application"
    = func: expr _ arg: expr {return new Application(func, arg)}

integer "integer number"
    = [0-9]+ {return createInteger(text())}

var "variable name"
    = name: name { return new Variable(name)}

kind "kind"
    = "TYPE" { return new Kind(); }

name "name"
    = $([a-z_]i+ [a-z0-9_]i*)

// optional whitespace
_ = [ \t\r\n]*

// mandatory whitespace
__ "space symbol" = [ \t\r\n]+