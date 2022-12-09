# Location Independent Representation of Lambda Calculus

## Abstract

This work allows to represent terms of lambda calculus without naming bound variables. Terms written in this representation are invariant to α-conversion and also invariant to location in formula's tree even when they have outer variables. Latter is not the case with De Bruijn indexes because indexes of outer variables change when term's depth in formula tree changes. It allows to perform β-reduction easily, meaning that when in term $(λx.T)P$ variable $x$ is substituted with term $P$, representation of term $P$ stays the same in all cases even when it is not closed. It also allows to perform more effective deduplication of terms because terms have the same representation in more cases then with De Bruijn indexes. This in turn allows for fast $O(1)$ α-equivalence comparisons even in cases when we compare open terms.

## Representation

This representation is mostly analogous to standard lambda calculus and so here I will describe only differences.

* In this representation variables do not have any value or name bound to them and so are denoted by $v$. 
Abstractions are of two kinds:
* boundable: $λT$ - can have bound variable in $T$,
* and unboundable: $ΛT$ - can't have bound variable.
* Applications are denoted by $T[s]P$ and have signed integer number $s$ called "shift" assigned to them meaning of which will be explained later.

Open variables of open terms have indices assigned to each of them which have about the same meaning as De Bruijn ones. Those indices are not written anywhere directly but can be deduced by term structure. Such indices $v_0, ..., v_{N-1}$ where $N > 0$ must satisfy conditions that $v_0 = 0$ and $v_i < v_{i+1}$. Each index $v_i$ has a set of variable occurrences $O_i$ bound to it. This set of occurrences corresponds to a set of all occurrences of open variable in standard lambda calculus.

Here we will describe the rules of calculation of variable indices by construction. Indices of internal terms will be denoted by $v_0, ..., v_{N-1}$, $w_0, ..., w_{M-1}$ and indices of constructed term by $v'_0, ..., v'_{N'-1}$. Sets of variable occurrences of internal terms will be denoted with $O_i$, $K_i$ and $O'_i$ for constructed term:
* Term $v$ has exactly one open variable with index $v'_0 = 0$ and $O'_0$ has only one occurrence.
* Unboundable abstraction $ΛT$ has the same variables as term $T$, meaning $v'_i = v_i$ and $O'_i = O_i$ where $0 \le i < N$.
* Boundable abstraction $λT$ has indices $$v'_i = v_{i+1} - v_1$$ and sets of occurrences $$O'_i = O_{i+1}$$ where $0 \le i < N - 1$. If $N > 0$ then occurrences $O_0$ become bound to the abstraction.
* Application $P[s]T$ has variable indices $$\set{v + l | v ∈ \set{v_0, ..., v_{N-1}}}\cup \set{w + k | w ∈ \set{w_0, ..., w_{M-1}}}$$ where $v_i$ are indices from $P$, $w_i$ indices from $T$. If $s = 0$ then indices of variables stay the same. If $s > 0$ then indices of variables from $P$ are increased by $s$. If $s < 0$ then indices from $T$ are increased by $-s$.

## Examples of terms

$v$ - has one open variable

$v[0]v$ - has two open variables having the same index. It is equivalent of term with one open variable with the same name.

$v[1]v$ - has two open variables. Left one has index 0 and right one index 1.

$λv[0]v$ - closed term with two variables bound to the same lambda.

$λv[1]v$ - open term. Left variable is bound to the lambda and right one is open

$λλv[1]v$ - closed term. Right variable is bound to the first lambda and left one to the second.

$λλv[-1]v$ - closed term. Left variable is bound to the first lambda and right one to the second.

$Λλv[-1]v$ - open term. Left variable is open and right one is bound to the small lambda.

Further we will prove several important properties of this representation.

## Isomorphism (unfinished)

Let us say that set $𝕃$ is a set of all formulas of lambda calculus and $𝕄$ is a set of all formulas in this representation. In order to use it to represent lambda calculus formulas we need to be sure that there are functions $f: 𝕃→𝕄$ and $f': 𝕄→𝕃$ such that $f'(f(T)) =_α T$.
...

## Calculation of variable bindings
...

## Conversion Algorithm
...

## β-reduction

...

Formula $λ(λv[1]v)[0]v$ in which the first variable is bound to the second lambda and the other variables are bound the first lambda will β-reduce to $λv[0]v$.

Formula $λλ(λv[1]v)[0](v[1]v)$ will β-reduce to $λλv[1]v[0]v$. Important moment is that term $v[1]v$ by which we substituted the variable haven't changed at all and we have correctly reduced formula.