using System.Collections.Generic;
using Logical.Ast;

namespace Logical.Parser;

public class AstBindStatesCalculator : FullAstVisitor<int, int>
{
    /// <summary>
    /// Is used to count variable references and to store variable depths 
    /// </summary>
    private readonly Dictionary<string, List<(int, int)>> _variables = new();

    public void Calculate(Ast.Nodes.Node ast)
    {
        ast.Visit(this);
    }

    protected override int AbstractionOrProductionOut(Ast.Nodes.AbstractionOrProduction node, Ast.BindingInfo<int> bindingInfo, int body, int? type, int? annotation)
    {
        node.IsUnbound = bindingInfo.Data == 0;
        return default;
    }
    protected override int UnboundAbstractionOrProductionOut(Ast.Nodes.AbstractionOrProduction node, int level, int body, int? type, int? annotation)
    {
        node.IsUnbound = true;
        return default;
    }
    protected override int Variable(Ast.Nodes.Variable node, List<Ast.BindingInfo<int>> bindings)
    {
        var binding = bindings[^1];
        binding.Data++;
        bindings[^1] = binding;
        return default;
    }

    protected override int OnDecimalLiteralError()
    {
        throw new System.NotImplementedException();
    }
}
