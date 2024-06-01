namespace Logical.Parser.Ast;

public interface IAstVisitor<TBinding, TNode>
    where TNode: struct
{
    /// <summary>
    /// Is called when we go out of abstraction or production node.
    /// Used when the abstraction or production potentially may be bound. 
    /// </summary>
    /// <param name="node">AST node</param>
    /// <param name="bindingInfo">Information about corresponding variable binding</param>
    /// <param name="body"></param>
    /// <param name="type">Data returned by type subtree visitor</param>
    /// <param name="annotation">Data returned by annotation subtree visitor</param>
    TNode AbstractionOrProductionOut(Nodes.AbstractionOrProduction node, BindingInfo<TBinding> bindingInfo, TNode body, TNode? type, TNode? annotation);

    /// <summary>
    /// Is called when we go out of abstraction or production node.
    /// Used when we already know that the abstraction or production is unbound
    /// </summary>
    /// <param name="node">AST node</param>
    /// <param name="level">Binding level of the node</param>
    /// <param name="body"></param>
    /// <param name="type"></param>
    /// <param name="annotation"></param>
    TNode UnboundAbstractionOrProductionOut(Nodes.AbstractionOrProduction node, int level, TNode body, TNode? type, TNode? annotation);
    
    TNode ApplicationOut(Nodes.Application node, TNode function, TNode argument, TNode? annotation);
    TNode PairOut(Nodes.Pair node, TNode left, TNode right);
    TNode ParenthesesOut(Nodes.Parentheses node, TNode @internal);

    /// <summary>
    /// Is called when we visit "variable" AST node 
    /// </summary>
    /// <param name="node">AST node</param>
    /// <param name="bindings">List of binding points to which variable with such name
    ///     may be bound (Usually to the last one)</param>
    /// <remarks>It should be taken into account that list of bindings may contain mixed
    /// abstraction and production bindings</remarks>
    TNode Variable(Nodes.Variable node, List<BindingInfo<TBinding>> bindings);
    
    TNode DecimalLiteral(Nodes.DecimalLiteral node);
    
    TBinding CreateBindingData();
}
