On the level of model the language consists of following basic constructions:

1. (?)Literals
2. Variables
3. Function definitions
4. Function applications
   //5. Function types

But on syntactical level it has additional constructions such as:

1. Operators
2.

## Literals

"Some String" - string literals. They produce lists of characters (@l())
132, -33 - integer literals. They can have any length, constrained only by size of memory.
1.22e33 - floating point literals. Also can have any size.

## Variables

x, X, var1, \_a - alfanumeric string (?) representing reference to variables with given name. If several variables have specified name in given scope then this construction will generate model with list of values of those variables.

## Function Applications

f 1, someFunction(1, x) - Application of function to some value. In second example the
value is tuple. Generates model: @a(а, 1), @a(someFunction, x) where @a is function capable of pattern matching of function arguments. In conjunction with variable lists it allows to implement function overloading.








