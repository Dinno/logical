{{
import { createInteger, createTypeAttr } from "../formalBuilders";
import { Kind, Application, Abstraction, Variable } from "../formalTree";
}}

start 
    = _ expr:expr _ { return expr; }

expr "expression"
    = fexpr / application / integer

fexpr "functional expression"
    = abstraction / var

application "function application"
    = r: applicationRec { r.reverse() } 

applicationRec "function application"
    = func: fexpr _ arg: expr {return new Application(func, arg)}

abstraction "function definition"
    = name: name _ ("=>" / ";") _ body: expr {return new Abstraction(name, body, [])}
    //= name: name _ ":" _ type: (kind / expr) _ ("=>" / ";") _ body: expr { return new Abstraction(name, body, [createTypeAttr(type)]) }

integer "integer number"
    = [0-9]+ {return createInteger(text())}

var "variable name"
    = name: name { return new Variable(name)}

name "name"
    = $([a-z_]i+ [a-z0-9_]i*)

// optional whitespace
_ = [ \t\r\n]*

// mandatory whitespace
__ "space symbol" = [ \t\r\n]+