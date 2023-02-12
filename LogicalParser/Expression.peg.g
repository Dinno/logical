@using Logical.Ast
@namespace Logical
@classname Parser
@ignorecase false

abstraction <Node>
    = n:name _ (";" / "=>") _ a:abstraction	{ new Abstraction(n, a, null) }
    / n:name _ "=" _ bound:application _ (";" / "=>") _ a:abstraction	{ new Application(new Abstraction(n, a, null), bound, null) }
    / application
    
application <Node> -memoize
    = fun:application arg:atom { new Application(fun, arg, null) }
    / atom
    
atom <Node>
    = "(" _ a:abstraction _ ")" { a }
    / DecimalLiteral
    / variable
    
variable <Node>
    = var:name { new Variable(var, null) }
    
name <string>
    = ([a-z_]i+ [a-z_0-9]i*)
    
DecimalLiteral <Node>
    = lit:([0-9]+ ("." [0-9]+)?) { new DecimalLiteral(lit, null) }  

_ <int>
    = [ \t]* { 0 } 
