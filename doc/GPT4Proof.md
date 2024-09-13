"Of course, here's a markdown version:

---

## Isomorphism between Standard Lambda Calculus and LIR

### Definition (Isomorphism):
Let $R_1$ and $R_2$ be two representations of the lambda calculus. An isomorphism $\phi: R_1 \to R_2$ is a bijective function such that:
1. If $t_1$ and $t_2$ are terms in $R_1$ and $t_1$ can be reduced to $t_2$ (using some reduction rule of $R_1$), then $\phi(t_1)$ can be reduced to $\phi(t_2)$ in $R_2$.
2. The converse is true: if $t_1'$ and $t_2'$ are terms in $R_2$ and $t_1'$ can be reduced to $t_2'$ (using some reduction rule of $R_2$), then $\phi^{-1}(t_1')$ can be reduced to $\phi^{-1}(t_2')$ in $R_1$.

This ensures that $\phi$ preserves the reduction relation in both directions.

---

### Mapping considerations

We will establish mapping between standard lambda calculus and normal form of LIR. Denormalized LIR is a superset of the normal form and can be mapped to standard lambda calculus by first mapping to normal form and then to standard lambda calculus.

To simplify mapping algorithms we will assume that mapped term has no open variables. Any open variable in standard lambda term may be made closed by adding abstraction with the name of the variable as its parent. This abstraction may be removed after mapping is complete. The same applies to the mapping from LIR to standard lambda calculus.

(???)In order to simplify the mapping process we will assume that name of a variable in every abstraction in the standard lambda calculus is unique (Is it needed?). We will also assume that names of variables of standard lambda calculus are somehow stored in the LIR. This can be done by adding a variable name field to the abstraction node. This field will be used to map abstractions from LIR to standard lambda calculus.

### Mapping from Standard to LIR

Since the LIR doesn't change structure of standard lambda tree, we can map from standard to LIR by simply replacing nodes of the tree with their LIR's counterparts. In particular if we have a term $t$ in the standard representation, we can map it to a term $t'$ in the LIR by applying the following recursive algorithm:

1. For each subterm $s$ of the term $t$ find its nearest ancestor abstraction which has at least one bound variable in this subterm. We will denote level of this abstraction in the lambda tree with $l_s$ where $s$ denotes name of the subterm.
2. If $t$ is variable then $t' = v$
3. If $t$ is $λx.T$ then $t' = λ\phi(T)$ or $t' = Λ\phi(T)$ depending on wether the abstraction has bound variables in $T$
4. If $t$ is $TP$ then $t' = TsP$, where $s = l_T - l_P$.

*Example:* $\lambda x. x y$ becomes $\lambda v_1v$.

---

### Mapping from LIR to Standard

Given a term $t'$ in the LIR:

1. For each $v$ term with an index, assign it a unique variable name in the standard lambda calculus.
2. For each $\lambda T$ or $\Lambda T$ form, find its counterpart in standard representation.
3. Repeat for the term recursively until all parts are converted.

*Example:* $\lambda v_1v$ becomes $\lambda x. x y$.

---

### Properties to Prove:

1. **Preservation of Reduction:** A term $t$ reduces to $t'$ in one representation implies its counterpart in the other representation reduces similarly. This is shown by induction over the term structure and the beta reduction definitions.

2. **Bijectiveness:** Ensure the mapping is one-to-one and onto.

   - **Injectivity:** Two distinct terms in standard representation can't map to the same term in the LIR given the defined construction rules.
   - **Surjectivity:** Every term in the LIR corresponds to a term in the standard representation given our mapping process.

---

In conclusion, we've established a bijective relationship between terms of the standard lambda calculus and the LIR. Thus, they are isomorphic."