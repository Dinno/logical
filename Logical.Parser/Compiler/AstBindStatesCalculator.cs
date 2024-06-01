using Application = Logical.Parser.Ast.Nodes.Application;

namespace Logical.Parser.Compiler;

public readonly struct AstBindStatesCalculator : Ast.IAstVisitor<int, int>
{
    /// <summary>
    /// Is used to count variable references and to store variable depths 
    /// </summary>
    private readonly Dictionary<string, List<(int, int)>> _variables = new();

    public AstBindStatesCalculator()
    {
    }

    public void Calculate(Ast.Nodes.Node ast)
    {
        var traversal = new Ast.AstTraversal<int, int, AstBindStatesCalculator>(this);
        traversal.Traverse(ast);
    }

    public int AbstractionOrProductionOut(Ast.Nodes.AbstractionOrProduction node, Ast.BindingInfo<int> bindingInfo, int body, int? type, int? annotation)
    {
        node.IsUnbound = bindingInfo.Data == 0;
        return 0;
    }
    public int UnboundAbstractionOrProductionOut(Ast.Nodes.AbstractionOrProduction node, int level, int body, int? type, int? annotation)
    {
        node.IsUnbound = true;
        return 0;
    }
    public int ApplicationOut(Application node, int function, int argument, int? annotation) => 0;
    public int PairOut(Ast.Nodes.Pair node, int left, int right) => 0;
    public int ParenthesesOut(Ast.Nodes.Parentheses node, int @internal) => 0;
    public int Variable(Ast.Nodes.Variable node, List<Ast.BindingInfo<int>> bindings)
    {
        var binding = bindings[^1];
        binding.Data++;
        bindings[^1] = binding;
        return 0;
    }
    public int DecimalLiteral(Ast.Nodes.DecimalLiteral node) => 0;
    public int CreateBindingData() => 0;
}
