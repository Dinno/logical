{{
import { createInteger, createTypeAttr } from "../formalBuilders";
import { Kind, Application, Abstraction, Variable } from "../formalTree";
}}

start 
    = _ expr:application _ { return expr; }

application "function application"
    = r:applicationRec { console.log(r); r.reverse() } 

applicationRec "function application"
    = func:fexpr _ arg:applicationRec {return new Application(func, arg)}
    / fexpr

fexpr "functional expression"
    = abstraction / var / integer  // Yes. Integer may potentially be function

abstraction "function definition"
    = name:name _ ("=>" / ";") _ body:application {return new Abstraction(name, body, [])}
    /// name:name _ ":" _ type:(kind / expr) _ ("=>" / ";") _ body:expr { return new Abstraction(name, body, [createTypeAttr(type)]) }

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
