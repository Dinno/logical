using Logical.Ast;
using Logical.Model;
using Application = Logical.Ast.Application;
using Node = Logical.Model.Node;
using Variable = Logical.Ast.Variable;

namespace Logical.Compiler;

public class Compiler : IAstVisitor<int, Node>
{
    public Node AbstractionOrProductionOut(AbstractionOrProduction node, BindingInfo<int> bindingInfo, Node body, Node? type,
        Node? annotation)
    {
        return new BoundAbstraction(body);
    }
    public Node UnboundAbstractionOrProductionOut(AbstractionOrProduction node, int level, Node body, Node? type, Node? annotation)
    {
        return new UnboundAbstraction(body);
    }
    public Node ApplicationOut(Application node, Node function, Node argument, Node? annotation)
    {
        throw new NotImplementedException();
    }
    public Node PairOut(Pair node, Node left, Node right)
    {
        throw new NotImplementedException();
    }
    public Node ParenthesesOut(Parentheses node, Node @internal)
    {
        throw new NotImplementedException();
    }
    public Node Variable(Variable node, List<BindingInfo<int>> bindings)
    {
        throw new NotImplementedException();
    }
    public Node DecimalLiteral(DecimalLiteral node)
    {
        throw new NotImplementedException();
    }
    public int CreateBindingData()
    {
        throw new NotImplementedException();
    }
}
