using Logical.Model;
using QuikGraph.Collections;

namespace Logical.Compiler;

public struct CompiledSubtree
{
    public Node Node;
    public FibonacciHeap<int, int> References;

    public CompiledSubtree(Node node, FibonacciHeap<int, int> references)
    {
        Node = node;
        References = references;
    }
}
public class Compiler : Ast.IAstVisitor<int, Node>
{
    public CompiledSubtree Compile(Ast.Node ast)
    {
        var traversal = new Ast.AstTraversal<int, CompiledSubtree, Compiler>(this);
        return traversal.Traverse(ast);
    }

    private static CompiledSubtree HandleBoundAbstraction(CompiledSubtree body, int level)
    {
        // Remove all variables referencing this abstraction
        foreach (var dummy in body.References.TakeWhile(pair => pair.Key == level)) { }

        return new CompiledSubtree(new BoundAbstraction(body.Node), body.References);
    }
    
    private static CompiledSubtree HandleAnnotation(CompiledSubtree annotated, CompiledSubtree annotation)
    {
        var leftLevel = -annotated.References.Top.Priority;
        var rightLevel = -annotation.References.Top.Priority;
        annotated.References.Merge(annotation.References);
        var mergedReferences = annotated;
        return new CompiledSubtree(new Annotation(annotated.Node, annotation.Node, ), mergedReferences);
    }

    public CompiledSubtree AbstractionOrProductionOut(Ast.AbstractionOrProduction node, Ast.BindingInfo<int> bindingInfo,
        CompiledSubtree body, CompiledSubtree? type,
        CompiledSubtree? annotation)
    {

        var abstraction = HandleBoundAbstraction(body, bindingInfo.Level);
        if (type.HasValue || annotation.HasValue)
            return HandleAnnotation(abstraction.References, abstraction.References);
        else
        {
            return abstraction;
        }
    }
    public Node UnboundAbstractionOrProductionOut(Ast.AbstractionOrProduction node, int level, Node body, Node? type, Node? annotation)
    {
        return new UnboundAbstraction(body);
    }
    public Node ApplicationOut(Ast.Application node, Node function, Node argument, Node? annotation)
    {
        throw new NotImplementedException();
    }
    public Node PairOut(Ast.Pair node, Node left, Node right)
    {
        throw new NotImplementedException();
    }
    public Node ParenthesesOut(Ast.Parentheses node, Node @internal)
    {
        throw new NotImplementedException();
    }
    public Node Variable(Ast.Variable node, List<Ast.BindingInfo<int>> bindings)
    {
        throw new NotImplementedException();
    }
    public Node DecimalLiteral(Ast.DecimalLiteral node)
    {
        throw new NotImplementedException();
    }
    public int CreateBindingData()
    {
        throw new NotImplementedException();
    }
}
