using LogicalParser.Ast;
using LogicalParser.Ast.Nodes;
using Application = LogicalParser.Ast.Nodes.Application;

namespace LogicalParser.Compiler;

public readonly struct AstBindStatesCalculator : IAstVisitor<int, int>
{
    /// <summary>
    /// Is used to count variable references and to store variable depths 
    /// </summary>
    private readonly Dictionary<string, List<(int, int)>> _variables = new();

    public AstBindStatesCalculator()
    {
    }

    public void Calculate(Node ast)
    {
        var traversal = new AstTraversal<int, int, AstBindStatesCalculator>(this);
        traversal.Traverse(ast);
    }

    public int AbstractionOrProductionOut(AbstractionOrProduction node, BindingInfo<int> bindingInfo, int body, int? type, int? annotation)
    {
        node.IsUnbound = bindingInfo.Data == 0;
        return 0;
    }
    public int UnboundAbstractionOrProductionOut(AbstractionOrProduction node, int level, int body, int? type, int? annotation)
    {
        node.IsUnbound = true;
        return 0;
    }
    public int ApplicationOut(Application node, int function, int argument, int? annotation) => 0;
    public int PairOut(Pair node, int left, int right) => 0;
    public int ParenthesesOut(Parentheses node, int @internal) => 0;
    public int Variable(Variable node, List<BindingInfo<int>> bindings)
    {
        var binding = bindings[^1];
        binding.Data++;
        bindings[^1] = binding;
        return 0;
    }
    public int DecimalLiteral(DecimalLiteral node) => 0;
    public int CreateBindingData() => 0;
}
