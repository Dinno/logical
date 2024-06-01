using System.Diagnostics;

namespace Logical.Parser.Ast.Nodes;

public class Production : AbstractionOrProduction
{
    public Node ArgumentType
    {
        get
        {
            Debug.Assert(Type is not null, "ArgumentType is empty!");
            return Type;
        }
    }
    public Node ResultType => Body;

    public Production(Node argumentType, Node resultType, string? variableName = null, Node? annotation = null) :
        base(resultType, variableName, argumentType, annotation)
    {
    }
}
