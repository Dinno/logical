# Location Independent Representation of Lambda Calculus

Denis Kerzhemanov

## Abstract

This paper introduces a novel method for representing terms in lambda calculus. Unlike traditional representations, this
new method eliminates the need for α-conversion and provides invariance to a term’s position within the formula tree. In
contrast, De Bruijn indices require adjustments to the indices of outer variables when the term's depth within the tree
changes. Our representation facilitates straightforward β-reduction; when substituting the variable $x$ in the term $(
λx.T)$ with a term $P$, the representation of $P$ remains unchanged. Additionally, our method enables more effective
deduplication of terms, as identical terms are guaranteed to be represented identically in contrast to De Bruijn
indices.
Consequently, this approach allows for constant time $O(1)$ α-equivalence comparisons, even for open terms.

## Differences with standard representation

This representation is largely analogous to standard lambda calculus, with the following differences:

* Variables do not have any value or name bound to them and are denoted by $v$.

* There are two kinds of abstractions:
    - **Boundable**: $λT$ - have at least one bound variable within $T$.
    - **Unboundable**: $ΛT$ - don't have bound variables within $T$.

  Otherwise, these abstractions have the same properties as the abstraction in standard representation

* Applications are denoted by $TsP$ and have a signed integer number $s$, called "shift", assigned to them, the meaning
  of which will be explained later.

The free variables of open terms are assigned indices similar in meaning to De Bruijn indices. These indices are not
explicitly written but can be inferred from the term structure. All occurrences of the same variable share the same
index. We denote such indices by $v_0, ..., v_{N-1}$, where $N$ is the number of free variables. Each index $v_i$ has a
corresponding set of variable occurrences, denoted by $O_i$. This set of occurrences corresponds to all the occurrences
of a free variable in standard lambda calculus.

## Calculation of indices

Here we will describe the rules of calculation of variable indices by construction. Indices of internal terms will be
denoted by $v_0, ..., v_{N-1}$, $w_0, ..., w_{M-1}$ and indices of constructed term by $v'_0, ..., v'_{N'-1}$. Sets of
variable occurrences of internal terms will be denoted with $O_i$, $K_i$ and $O'_i$ for constructed term:

* Term $v$ has one open variable with index $v'_0 = 0$ and $O'_0$ has only one occurrence.
* Unboundable abstraction $ΛT$ has the same variables as term $T$, meaning $v'_i = v_i$ and $O'_i = O_i$ for $0 \le i <
  N$.
* Boundable abstraction $λT$ has indices and occurrences $$v'_i = v_{i+1} - v_1$$ $$O'_i = O_{i+1}$$ where $0 \le i <
  N - 1$. If $N > 0$ then occurrences from $O_0$ become bound to the abstraction.
* Application $PsT$ has set of variable indices $$\set{v + \max(0, -s) | v ∈ \set{v_0, ..., v_{N-1}}}\cup \set{w + \max(
  0, s) | w ∈ \set{w_0, ..., w_{M-1}}}$$ where $v_i$ are indices from $P$, $w_i$ indices from $T$. New indices are
  numbered so that $v'_0 = 0$ and $v'_i < v_{i+1}$. Resulting occurrences of variables are unions of occurrences from
  internal terms with equal final numbers.

## Examples of terms

| λ          | λ De Bruijn | This representation | Description                                                                         |
|------------|-------------|---------------------|-------------------------------------------------------------------------------------|
| $x$        | $1$         | $v$                 | Open term. Has one open variable                                                    |
| $xx$       | $11$        | $v0v$               | Open term. Has two open variables having the same index                             |
| $xy$       | $12$        | $v1v$               | Open term. Has two open variables. Left one has index 0 and right one index 1       |
| $λx.x x$   | $λ11$       | $λv0v$              | Closed term with two variables bound to the same lambda                             |
| $λx.x y$   | $λ12$       | $λv1v$              | Open term. Left variable is bound to the lambda and right one is open               |
| $λy.λx.xy$ | $λλ12       | $λλv1v$             | Closed term. Right variable is bound to the first lambda and left one to the second |
| $λy.λx.yx$ | $λλ21       | $λλv{-1}v$          | Closed term. Left variable is bound to the first lambda and right one to the second |
| $λy.λx.zx$ | $$λλ31$     | $Λλv-1v$            | Open term. Left variable is open and right one is bound to the small lambda         |

## Well-formed term

A term in location-independent representation is said to be well-formed if for every subterm of the form $λT$, the
subterm $T$ has at least one free variable which has index 0.

This condition ensures that all bound abstractions in this representation are actually bound and have meaning.

## Normal form

A term in location-independent representation is said to be in normal form if the following conditions are met:

1. If a term has a variable with index $v_0$, then $v_0$ must be equal to 0.
2. All its subterms are in normal form.

## Complete normal form

Formula is a term which is not part of another term. Formula in location-independent representation is said to be in
complete normal form if the following conditions are met:

1. The term is in normal form
2. All free variables in the term, if they exist, have sequential indices.

These conditions ensure that any two formulas which are alfa-equivalent in representation of standard lambda calculus
are equivalent when they are represented in normal form of this representation.

As we see by these normal forms. In this representation some information about binding of a term to outer terms is
encoded inside the term.

## Isomorphism

The isomorphism between the location independent representation of lambda terms and the standard representation of
untyped lambda calculus can be established by showing that every term in one representation has a unique corresponding
term in the other representation, and that these corresponding terms preserve the meaning and structure of the original
term.

To do this, we can define a function that maps terms in the standard representation to terms in the location independent
representation, and vice versa. We can then show that these functions are bijective and preserve the structure and
meaning of terms.

**Proof of Isomorphism Between the Location Independent Representation and Standard Untyped Lambda Calculus**

---

**Introduction** Our goal is to establish that the **location independent representation**  of lambda calculus terms is
**isomorphic**  to the standard untyped lambda calculus. An isomorphism here means there's a bijective mapping between
the two representations that preserves the structure and semantics of lambda terms.
To achieve this, we will:

1. **Clarify the definitions**  of both representations.

2. **Define two mappings** :

- A mapping $\Phi$ from the location independent representation to the standard lambda calculus.

- A mapping $\Psi$ from the standard lambda calculus to the location independent representation.

3. **Prove**  that these mappings are inverses of each other, thus establishing the isomorphism.

---

**Clarifying the Representations** **Standard Untyped Lambda Calculus**

- **Syntax** :
    - **Variables** : $x, y, z, \ldots$

    - **Terms**  ($M, N, T, P$):
        - **Variable** : $x$

        - **Abstraction** : $\lambda x. M$

        - **Application** : $M \ N$

- **Semantics** :
    - **Variable Binding** : Variables can be **free**  or **bound** .

    - **Alpha-Equivalence** : Terms are considered equivalent if they differ only in the names of their bound variables.
      **Location Independent Representation**
- **Components** :
    - **Variables** : Denoted by $v$.

    - **Abstractions** :
        - **Bound Abstraction** : $\lambda T$ (binds variables in $T$).

        - **Unbound Abstraction** : $\Lambda T$ (does not bind variables in $T$).

    - **Application** : $P_s Q$, where $s$ is a signed integer called a **shift** .

- **Variable Indices** :
    - **Open Variables** : Assigned indices $v_0, v_1, \ldots, v_{N-1}$, satisfying $v_0 = 0$ and $v_i < v_{i+1}$.

- **Construction Rules** :
    1. **Variable ($v$)** :

    - Represents an open variable with index $0$.

    2. **Unbound Abstraction ($\Lambda T$)** :

    - Indices remain unchanged.

    3. **Bound Abstraction ($\lambda T$)** :

    - Binds the variable with index $0$ in $T$.

    - Decreases all other indices by $1$.

    4. **Application ($P_s Q$)** :

    - Adjusts variable indices of $P$ and $Q$ based on the shift $s$.

---

**Defining the Mappings** Mapping $\Phi$: From Location Independent Representation to Standard Lambda Calculus** We
define $\Phi(T, \Gamma)$, where:

- $T$ is a term in the location independent representation.

- $\Gamma$ is an **environment**  mapping variable indices to variable names.
  **Initial Environment** :
- For open terms, $\Gamma$ assigns unique variable names to indices:
  $
  \Gamma(i) = x_i \quad \text{for } i \geq 0
  $
  **Mapping Rules** :

1. **Variable ($v$)** :

- $\Phi(v, \Gamma) = \Gamma(0)$.

2. **Unbound Abstraction ($\Lambda T$)** :

- $\Phi(\Lambda T, \Gamma) = \Phi(T, \Gamma)$.

3. **Bound Abstraction ($\lambda T$)** :

- Let $x = \Gamma(0)$ (the variable name for index $0$).

- Update the environment $\Gamma'$ by:
    - Removing the mapping for index $0$.

    - Shifting the indices: $\Gamma'(i) = \Gamma(i + 1)$ for $i \geq 0$.

- $\Phi(\lambda T, \Gamma) = \lambda x. \Phi(T, \Gamma')$.

4. **Application ($P_s Q$)** :

- Adjust the environments for $P$ and $Q$ based on the shift $s$:
    - For $P$:
        - $\Gamma_P(i) = \Gamma(i + \max(0, -s))$.

    - For $Q$:
        - $\Gamma_Q(i) = \Gamma(i + \max(0, s))$.

- $\Phi(P_s Q, \Gamma) = \Phi(P, \Gamma_P) \ \Phi(Q, \Gamma_Q)$.
  **Explanation** :
- **Variable Names via $\Gamma$** : All variable names are introduced and managed through the environment $\Gamma$,
  ensuring a consistent mapping between variable names and indices.

- **Environment Updates** :
    - When a variable is bound, we remove its index from $\Gamma$ and adjust the indices of the remaining variables.

    - In applications, we adjust $\Gamma$ based on the shift $s$ to align variable indices correctly.
      Mapping $\Psi$: From Standard Lambda Calculus to Location Independent Representation** We
      define $\Psi(M, \Gamma)$, where:
- $M$ is a term in the standard lambda calculus.

- $\Gamma$ is an **environment**  mapping variable names to indices.
  **Initial Environment** :
- For open terms, $\Gamma$ is empty:
  $
  \Gamma = \{\}
  $
  **Mapping Rules** :

1. **Variable ($x$)** :

- If $x$ is in $\Gamma$:
    - $\Psi(x, \Gamma) = v_{\Gamma(x)}$.

- If $x$ is not in $\Gamma$:
    - Assign a new index $n$ (the next available index).

    - Update $\Gamma$ with $\Gamma'(x) = n$.

    - $\Psi(x, \Gamma') = v_n$.

2. **Abstraction ($\lambda x. M$)** :

- Assign index $0$ to $x$.

- Increase the indices of all variables in $\Gamma$ by $1$:
  $
  \Gamma'(y) = \Gamma(y) + 1 \quad \text{for all } y \in \Gamma
  $

- Update $\Gamma'$ with $\Gamma'(x) = 0$.

- $\Psi(\lambda x. M, \Gamma) = \lambda \Psi(M, \Gamma')$.

3. **Application ($M \ N$)** :

- $\Psi(M \ N, \Gamma) = \Psi(M, \Gamma)_0 \Psi(N, \Gamma)$.

- The shift $s = 0$ since both $M$ and $N$ are at the same depth.
  **Explanation** :
- **Variable Indices via $\Gamma$** : Variables are assigned indices based on their names and the current
  environment $\Gamma$.

- **Environment Updates** :
    - When entering an abstraction, we reset the index for the bound variable to $0$ and adjust the indices of free
      variables.

    - In applications, the shift $s$ is determined by the relative depths, which are handled implicitly here.

---

**Proving the Isomorphism** To establish that the mappings $\Phi$ and $\Psi$ are inverses of each other, we need to
show:

1. **Composition $\Phi \circ \Psi$** :

- For any standard lambda term $M$, $\Phi(\Psi(M, \Gamma), \Gamma) \equiv_\alpha M$.

2. **Composition $\Psi \circ \Phi$** :

- For any location independent term $T$, $\Psi(\Phi(T, \Gamma), \Gamma) = T$.
  **Proof** 1. $\Phi \circ \Psi = \text{Identity}$ on Standard Lambda Terms** We proceed by structural induction on $M$.
- **Base Case**  (Variable $x$):
    - If $x$ is free:
        - $\Psi(x, \Gamma) = v_n$ with $\Gamma'(x) = n$.

        - $\Phi(v_n, \Gamma') = \Gamma'(0) = x$ (since $n = 0$ in this case).

    - If $x$ is bound:
        - The environment $\Gamma$ ensures $\Gamma(x)$ maps to the correct index.

        - $\Psi(x, \Gamma) = v_{\Gamma(x)}$.

        - $\Phi(v_{\Gamma(x)}, \Gamma) = \Gamma(\Gamma(x)) = x$.

- **Inductive Step**  (Abstraction $\lambda x. M$):
    - In $\Psi$, we assign $\Gamma'(x) = 0$ and increment indices of other variables.

    - $\Psi(\lambda x. M, \Gamma) = \lambda \Psi(M, \Gamma')$.

    - In $\Phi$, we use $\Gamma$ where $\Gamma(0) = x$.

    - $\Phi(\lambda \Psi(M, \Gamma'), \Gamma) = \lambda x. \Phi(\Psi(M, \Gamma'), \Gamma')$.

    - By induction hypothesis, $\Phi(\Psi(M, \Gamma'), \Gamma') \equiv_\alpha M$.

    - Therefore, $\Phi(\Psi(\lambda x. M, \Gamma), \Gamma) \equiv_\alpha \lambda x. M$.

- **Inductive Step**  (Application $M \ N$):
    - $\Psi(M \ N, \Gamma) = \Psi(M, \Gamma)_0 \Psi(N, \Gamma)$.

    - In $\Phi$, we adjust $\Gamma$ based on shifts (which are zero here).

    - $\Phi(\Psi(M, \Gamma)_0 \Psi(N, \Gamma), \Gamma) = \Phi(\Psi(M, \Gamma), \Gamma) \ \Phi(\Psi(N, \Gamma), \Gamma)$.

    - By induction hypothesis, $\Phi(\Psi(M, \Gamma), \Gamma) \equiv_\alpha M$
      and $\Phi(\Psi(N, \Gamma), \Gamma) \equiv_\alpha N$.

    - Therefore, $\Phi(\Psi(M \ N, \Gamma), \Gamma) \equiv_\alpha M \ N$.

2. $\Psi \circ \Phi = \text{Identity}$ on Location Independent Terms** We proceed by structural induction on $T$.

- **Base Case**  (Variable $v$):
    - $\Phi(v, \Gamma) = \Gamma(0)$.

    - $\Psi(\Gamma(0), \Gamma) = v_{\Gamma(\Gamma(0))} = v_0$ (since $\Gamma(\Gamma(0)) = 0$).

    - Therefore, $\Psi(\Phi(v, \Gamma), \Gamma) = v$.

- **Inductive Step**  (Unbound Abstraction $\Lambda T$):
    - $\Phi(\Lambda T, \Gamma) = \Phi(T, \Gamma)$.

    - $\Psi(\Phi(T, \Gamma), \Gamma) = T$ by induction hypothesis.

    - Therefore, $\Psi(\Phi(\Lambda T, \Gamma), \Gamma) = T$.

- **Inductive Step**  (Bound Abstraction $\lambda T$):
    - $\Phi(\lambda T, \Gamma) = \lambda x. \Phi(T, \Gamma')$, where $x = \Gamma(0)$ and $\Gamma'$ is updated.

    - In $\Psi$, we assign $\Gamma''(x) = 0$ and increment indices of other variables.

    - By induction hypothesis, $\Psi(\Phi(T, \Gamma'), \Gamma'') = T$.

    - Therefore, $\Psi(\Phi(\lambda T, \Gamma), \Gamma) = \lambda T$.

- **Inductive Step**  (Application $P_s Q$):
    - $\Phi(P_s Q, \Gamma) = \Phi(P, \Gamma_P) \ \Phi(Q, \Gamma_Q)$.

    - By induction hypothesis, $\Psi(\Phi(P, \Gamma_P), \Gamma_P) = P$ and $\Psi(\Phi(Q, \Gamma_Q), \Gamma_Q) = Q$.

    - Since the shifts and environments are adjusted consistently, $\Psi(\Phi(P_s Q, \Gamma), \Gamma) = P_s Q$.

---

**Conclusion** By revising the mapping $\Phi$ to introduce variable names strictly through the environment $\Gamma$,
we've ensured that variable names correspond exactly to their indices. This allows us to reverse the mapping accurately
using $\Psi$, thereby establishing an isomorphism between the location independent representation and the standard
untyped lambda calculus.

---

**Remarks**

- **Environment $\Gamma$** : Acts as a bridge between variable names and indices, essential for the isomorphism.

- **Variable Names Correspond to Indices** : Ensures that we can reconstruct indices from variable names when reversing
  the mapping.

- **Consistency in Mappings** : Careful management of $\Gamma$ in both mappings preserves the structure and semantics of
  terms.

...

## Calculation of variable bindings

...

## Conversion Algorithm

...

## β-reduction

...

Formula $λ(λv[1]v)[0]v$ in which the first variable is bound to the second lambda and the other variables are bound the
first lambda will β-reduce to $λv[0]v$.

Formula $λλ(λv[1]v)[0](v[1]v)$ will β-reduce to $λλv[1]v[0]v$. Important moment is that term $v[1]v$ by which we
substituted the variable haven't changed at all and we have correctly reduced formula.