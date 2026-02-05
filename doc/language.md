On the model level the language consists of following basic constructions:

1. Variables (x)
2. Function definitions (x => y)
3. Function applications (x y)
4. Types (Prop, Set, Type 0 , Type 1...)
5. Productions (x: T -> P)

But on syntactical level it has additional constructions:

1. Operators and constructors
2. Pattern matchers
3. Literals
4. Imports and exports

## PureTS prototype (F#)

Minimal Haskell-like surface syntax for the prototype:

- `let x = expr in expr`
- Lambdas: `\\x y -> expr`
- Function application by space: `f x y`
- Records: `{a = expr, b = expr}` and projection `expr.a`
- Literals: numbers, booleans, strings

IR uses de Bruijn indices and includes a literal layer for numbers/booleans/strings
to keep prototyping practical. Encodings like Peano/Church remain optional.

## Operators

- +, -, \*, /, % - arithmetics
- ==, !=, <, <=, >, >= - comparison
- [], . - indexers

## Constructors

- @{a: Int, b: String} - structure types
- {a: 1, b: "2"} - structures
- Int \* String - tuple types
- (2, "dd") - tuples
- Int[] - list types
- [2, 3, 4] - lists

## Literals

"Some String" - string literals. They produce lists of characters (@l())
132, -33 - integer literals. They can have any length, constrained only by size of memory.
1.22e33 - floating point literals. Also can have any size.

## Variables

x, X, var1, \_a - alfa-numeric string (?) representing reference to variables with a given name.
x^ - this construction ('^' symbolizes vector) allows to capture all variables with the name 'x' visible in a given
scope. It will generate model with list of values of those variables.

## Function declarations

### Examples

`x => x`, `x, y => x * y`, `_ => null`, `x; y => x * y`, `(1, x) => x`

## Data type declarations

```
data List T: Set
    | nil
    | cons: T, List T;
```

is equivalent to

```
inductive List (T: Set): Set
    | nil: List T
    | cons: T -> List T -> List T;
```

```
data Tree
    | node: Forest
with Forest
    | emptyForest
    | consForest: Tree, Forest;
```

is equivalent to

```
inductive Tree: Set
    | node: Forest -> Tree
with Forest: Set
    | emptyForest: Forest
    | consForest: Tree -> Forest -> Forest;
```

## Fixpoints

The language allows to define Y-combinator. It is a function which allows to define recursive functions.
As this function is not typeable in the dependently typed language, it is defined using untyped variant of the language.
Y-combinator is defined in the standard library as follows:

```
yCombinator: (A -> B) -> B = untyped f => (x => f (x x)) (x => f (x x))
```

'untyped' is a keyword which allows to define untyped expression.

### Semantic tree generation

Simple function declaration generates abstraction annotated by name of declared variable. `x => x` converts
to $(\lambda v)_{name("x")}$. Underline instead of variable name leads to removal of name annotation and makes resulting
abstraction unboundable: `_ => null` $\Rightarrow$ $\Lambda v$

Pattern in a left part of function definition leads to use of destructuring function in semantic tree.
`x, y => x * y` $\Rightarrow$ $(\lambda v)(v_0, v_1)$

## Function Applications

`f 1`, `someFunction(1, x)` - Application of function to some value. In second example the
value is tuple. Generates model: @apply(f, 1), @apply(someFunction, @tuple (@pair 1 x)) where @apply is function capable
of pattern matching of function arguments. In conjunction with variable lists it allows to implement function
overloading.

## Imports

Imports are done using special type of function which must always have constant string argument. Syntax:
module1 = import "module1";

## Exports

Exports mark variables to be included into structure which will become a module's value
export f = x => x _ x;
Will be automatically converted into
f = x => x _ x;
{f}
To finally become:
{f: x => x \* x}
In first version of the language exports are not needed, but should be added later.
