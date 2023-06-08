# Location Independent Representation of Lambda Calculus

## Abstract

This work allows to represent terms of lambda calculus without naming bound variables. Terms written in this representation are invariant to α-conversion and also invariant to location in formula's tree even when they have outer variables. Latter is not the case with De Bruijn indexes because indexes of outer variables change when term's depth in formula tree changes. It allows to perform β-reduction easily, meaning that when in term $(λx.T)P$ variable $x$ is substituted with term $P$, representation of term $P$ stays the same in all cases even when it is not closed. It also allows to perform more effective deduplication of terms because terms have the same representation in more cases then with De Bruijn indexes. This in turn allows for fast $O(1)$ α-equivalence comparisons even in cases when we compare open terms.

## Representation

This representation is mostly analogous to standard lambda calculus and so here I will describe only differences.

* In this representation variables do not have any value or name bound to them and so are denoted by $v$. 
Abstractions are of two kinds:
* boundable: $λT$ - can have bound variable in $T$,
* and unboundable: $ΛT$ - can't have bound variable.
* Applications are denoted by $TsP$ and have signed integer number $s$ called "shift" assigned to them meaning of which will be explained later.

Open variables of open terms have indices assigned to each of them which have about the same meaning as De Bruijn ones. Those indices are not written anywhere directly but can be deduced by term structure. Such indices $v_0, ..., v_{N-1}$ where $N > 0$ must satisfy conditions that $v_0 = 0$ and $v_i < v_{i+1}$. Each index $v_i$ has a set of variable occurrences $O_i$ bound to it. This set of occurrences corresponds to a set of all occurrences of open variable in standard lambda calculus.

Here we will describe the rules of calculation of variable indices by construction. Indices of internal terms will be denoted by $v_0, ..., v_{N-1}$, $w_0, ..., w_{M-1}$ and indices of constructed term by $v'_0, ..., v'_{N'-1}$. Sets of variable occurrences of internal terms will be denoted with $O_i$, $K_i$ and $O'_i$ for constructed term:
* Term $v$ has exactly one open variable with index $v'_0 = 0$ and $O'_0$ has only one occurrence.
* Unboundable abstraction $ΛT$ has the same variables as term $T$, meaning $v'_i = v_i$ and $O'_i = O_i$ for $0 \le i < N$.
* Boundable abstraction $λT$ has indices and occurrences $$v'_i = v_{i+1} - v_1$$ $$O'_i = O_{i+1}$$ where $0 \le i < N - 1$. If $N > 0$ then occurrences from $O_0$ become bound to the abstraction.
* Application $PsT$ has set of variable indices $$\set{v + \max(0, -s) | v ∈ \set{v_0, ..., v_{N-1}}}\cup \set{w + \max(0, s) | w ∈ \set{w_0, ..., w_{M-1}}}$$ where $v_i$ are indices from $P$, $w_i$ indices from $T$. New indices are numbered so that $v'_0 = 0$ and $v'_i < v_{i+1}$. Resulting occurrences of variables are unions of occurrences from internal terms with equal final numbers.

## Examples of terms

| λ | λ De Bruijn  | This representation | Description |
|---|---|---|---|
| $x$ | $1$ | $v$ | Open term. Has one open variable |
| $xx$ | $11$ | $v0v$ | Open term. Has two open variables having the same index |
| $xy$ | $12$ | $v1v$ | Open term. Has two open variables. Left one has index 0 and right one index 1 |
| $λx.x x$ | $λ11$ | $λv0v$ | Closed term with two variables bound to the same lambda |
| $λx.x y$ | $λ12$ | $λv1v$ | Open term. Left variable is bound to the lambda and right one is open |
| $λy.λx.xy$ | $λλ12 | $λλv1v$ | Closed term. Right variable is bound to the first lambda and left one to the second |
| $λy.λx.yx$ | $λλ21 | $λλv{-1}v$ | Closed term. Left variable is bound to the first lambda and right one to the second |
| $λy.λx.zx$ | $$λλ31$ | $Λλv-1v$ | Open term. Left variable is open and right one is bound to the small lambda |

## Normal form
Formula in location independent representation is said to be in normal form iff:
1. For all of its subterms of form $λT$ apply conditions that its subterm $T$ has at least one open variable and index $v_1$ of term $T$ if it exists is equal to $1$.
2. All formula's open variables if they exist have sequential indices.

## Isomorphism

<!-- Let us say that set $𝕃$ is a set of formulas of lambda calculus and $𝕄$ is a set of formulas in this representation. In order to use this representation we need to be sure that there are functions $f: 𝕃→𝕄$ and $f^{-1}: 𝕄→𝕃$ such that $f^{-1}(f(T)) =_α T$. In which case we can say that two representations are isomorphic.

Actually we will show how to construct function that maps set of alfa-equivalence classes of formulas of lambda calculus $𝕃_α$ to set of formula of this representation in normal form $𝕄_n$ so that this mapping $f : 𝕃_α → 𝕄_n$ is a bijection.

Function t:  -->

The isomorphism between the location independent representation of lambda terms and the standard representation of untyped lambda calculus can be established by showing that every term in one representation has a unique corresponding term in the other representation, and that these corresponding terms preserve the meaning and structure of the original term.

To do this, we can define a function that maps terms in the standard representation to terms in the location independent representation, and vice versa. We can then show that these functions are bijective and preserve the structure and meaning of terms.

To map a term in the standard representation to a term in the location independent representation, we can use the following algorithm:

a. Start with an empty set of bound variables and a counter initialized to 0.

b. For each abstraction of the form $λx.M$, where $M$ is a subterm:

```
 i. If $x$ is not already in the set of bound variables, add it to the set and assign it a unique index $i$.
 
 ii. Replace $x$ with the index $i$ in $M$, and recursively apply this algorithm to $M$.
 
 iii. Return the term $λT$, where $T$ is the result of applying the algorithm to $M$.
```

c. For each application of the form $MN$, recursively apply this algorithm to $M$ and $N$, and return the term $TsP$, where $s$ is the difference between the number of abstractions in $M$ and the number of abstractions in $N$.

d. For each variable $x$, check whether it is in the set of bound variables. If it is, replace it with its index, otherwise leave it as it is.

This algorithm maps every term in the standard representation to a term in the location independent representation, and ensures that variables in the same scope are represented by the same index.

To map a term in the location independent representation to a term in the standard representation, we can use the following algorithm:

a. For each abstraction of the form $λT$:

```
 i. Find the index $i$ of the bound variable in $T$.
 
 ii. Replace the index $i$ with a fresh variable $x$, and recursively apply this algorithm to $T$.
 
 iii. Return the term $λx.M$, where $M$ is the result of applying the algorithm to $T$.
```

b. For each application of the form $TsP$, recursively apply this algorithm to $T$ and $P$, and return the term $MN$, where $N$ is the result of applying the algorithm to $P$ and $M$ is the result of applying the algorithm to $T$ with all occurrences of indices replaced by their corresponding variables.

c. For each variable $v$ with index $i$, replace $v$ with the $i$-th bound variable in the current scope.

This algorithm maps every term in the location independent representation to a term in the standard representation, and ensures that variables are replaced by the correct names in each scope.

To show that these functions are bijective and preserve the structure and meaning of terms, we need to prove two things:

For every term $M$ in the standard representation, the algorithm in step 1 produces a unique term $M'$ in the location independent representation, and for every term $N$ in the location independent representation, the algorithm in step 2 produces a unique term $N'$ in the standard representation.

For any two terms $M$ and $N$ in the standard

...


## Calculation of variable bindings
...

## Conversion Algorithm
...

## β-reduction

...

Formula $λ(λv[1]v)[0]v$ in which the first variable is bound to the second lambda and the other variables are bound the first lambda will β-reduce to $λv[0]v$.

Formula $λλ(λv[1]v)[0](v[1]v)$ will β-reduce to $λλv[1]v[0]v$. Important moment is that term $v[1]v$ by which we substituted the variable haven't changed at all and we have correctly reduced formula.