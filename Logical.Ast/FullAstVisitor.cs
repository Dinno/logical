using System.Collections.Generic;

namespace Logical.Parser.Ast;

public abstract class FullAstVisitor<TBinding, TNode> : IAstVisitor<TNode>
    where TNode : struct
    where TBinding : struct
{
    private int _level;

    /// <summary>
    /// Is used to count variable references and to store variable depths 
    /// </summary>
    private readonly Dictionary<string, List<BindingInfo<TBinding>>> _variables = new();

    public FullAstVisitor()
    {
    }

    public FullAstVisitor(int initialLevel, Dictionary<string, List<BindingInfo<TBinding>>> initialVariables)
    {
        _level = initialLevel;
        _variables = initialVariables;
    }


    public TNode Visit(Nodes.AbstractionOrProduction node)
    {
        TNode? type = default;
        if (node.Type is not null)
            type = node.Type.Visit(this);

        TNode? annotation = default;
        if (node.Annotation is not null)
            annotation = node.Annotation.Visit(this);

        var variableName = node.VariableName;

        if (node.IsUnbound || variableName is null)
        {
            var body = node.Body.Visit(this);
            return UnboundAbstractionOrProductionOut(node, _level, body, type,
                annotation);
        }

        if (!_variables.TryGetValue(variableName, out var varList))
        {
            varList = new List<BindingInfo<TBinding>>();
            _variables.Add(variableName, varList);
        }

        var varIndex = varList.Count;
        varList.Add(new BindingInfo<TBinding>(_level, CreateBindingData()));
        _level++;

        var body2 = node.Body.Visit(this);

        _level--;
        var bindingInfo = varList[varIndex];
        varList.RemoveAt(varIndex);

        return AbstractionOrProductionOut(node, bindingInfo, body2, type, annotation);
    }

    public TNode Visit(Nodes.Application node)
    {
        var function = node.Function.Visit(this);
        var argument = node.Argument.Visit(this);
        TNode? annotation = default;
        if (node.Annotation is not null)
            node.Annotation.Visit(this);
        return ApplicationOut(node, function, argument, annotation);
    }

    public TNode Visit(Nodes.Pair node)
    {
        var left = node.Left.Visit(this);
        var right = node.Right.Visit(this);
        if (node.Annotation is not null)
            node.Annotation.Visit(this);
        return PairOut(node, left, right);
    }

    public TNode Visit(Nodes.Parentheses node)
    {
        var intern = node.Node.Visit(this);
        if (node.Annotation is not null)
            node.Annotation.Visit(this);
        return ParenthesesOut(node, intern);
    }

    public TNode Visit(Nodes.Variable node)
    {
        var varList = _variables[node.Name];
        return Variable(node, varList);
    }

    public virtual TNode Visit(Nodes.DecimalLiteral node) => default;

    /// <summary>
    /// Is called when we go out of abstraction or production node.
    /// Used when the abstraction or production potentially may be bound. 
    /// </summary>
    /// <param name="node">AST node</param>
    /// <param name="bindingInfo">Information about corresponding variable binding</param>
    /// <param name="body"></param>
    /// <param name="type">Data returned by type subtree visitor</param>
    /// <param name="annotation">Data returned by annotation subtree visitor</param>
    protected virtual TNode AbstractionOrProductionOut(Nodes.AbstractionOrProduction node,
        BindingInfo<TBinding> bindingInfo, TNode body,
        TNode? type, TNode? annotation) =>
        default;

    /// <summary>
    /// Is called when we go out of abstraction or production node.
    /// Used when we already know that the abstraction or production is unbound
    /// </summary>
    /// <param name="node">AST node</param>
    /// <param name="level">Binding level of the node</param>
    /// <param name="body"></param>
    /// <param name="type"></param>
    /// <param name="annotation"></param>
    protected virtual TNode UnboundAbstractionOrProductionOut(Nodes.AbstractionOrProduction node, int level,
        TNode body, TNode? type,
        TNode? annotation) =>
        default;

    protected virtual TNode ApplicationOut(Nodes.Application node, TNode function, TNode argument, TNode? annotation) =>
        default;

    protected virtual TNode PairOut(Nodes.Pair node, TNode left, TNode right) => default;

    protected virtual TNode ParenthesesOut(Nodes.Parentheses node, TNode @internal) => default;

    /// <summary>
    /// Is called when we visit "variable" AST node 
    /// </summary>
    /// <param name="node">AST node</param>
    /// <param name="bindings">List of binding points to which variable with such name
    ///     may be bound (Usually to the last one)</param>
    /// <remarks>It should be taken into account that list of bindings may contain mixed
    /// abstraction and production bindings</remarks>
    protected virtual TNode Variable(Nodes.Variable node, List<BindingInfo<TBinding>> bindings) => default;

    protected virtual TBinding CreateBindingData() => default;
}