On the model level the language consists of following basic constructions:

1. Variables (x)
2. Function definitions (x => y)
3. Function applications (x y)
4. Types (TYPE)
5. Productions (x -> y)

But on syntactical level it has additional constructions:

1. Operators and constructors
2. Pattern matchers
3. Literals
4. Imports and exports

## Operators

* +, -, *, /, % - arithmetics
* ==, !=, <, <=, >, >= - comparison
* [], . - indexers

## Constructors

* @{a: Int, b: String} - structure types
* {a: 1, b: "2"} - structures
* Int * String - tuple types
* (2, "dd") - tuples
* Int[] - array types
* [2, 3, 4] - arrays

## Literals

"Some String" - string literals. They produce lists of characters (@l())
132, -33 - integer literals. They can have any length, constrained only by size of memory.
1.22e33 - floating point literals. Also can have any size.

## Variables

x, X, var1, \_a - alfanumeric string (?) representing reference to variables with given name. If several variables have specified name in given scope then this construction will generate model with list of values of those variables.

## Function declarations

### Examples
`x => x`, `x, y => x * y`, `_ => null`, `x; x * x`, `(1, x) => x`

### Generation
Simple function declaration generates abstraction annotated by name of declared variable. `x => x` converts to $(\lambda v)_{name("x")}$. Underline instead of variable name leads to removal of name annotation and makes resulting abstraction unboundable: `_ => null` $\rArr$ $\Lambda v$

Pattern in a left part of function definition leads to generation of pattern matching proxy???

## Function Applications

`f 1`, `someFunction(1, x)` - Application of function to some value. In second example the
value is tuple. Generates model: @a(а, 1), @a(someFunction, x) where @a is function capable of pattern matching of function arguments. In conjunction with variable lists it allows to implement function overloading.

## Imports

Imports are done using special type of function which must always have constant string argument. Syntax:
module1 = import "module1";

## Exports
Exports mark variables to be included into structure which will become a module's value
export f = x => x * x;
Will be automatically converted into
f = x => x * x;
{f}
To finally become:
{f: x => x * x}
In first version of the language exports are not needed, but should be added later.