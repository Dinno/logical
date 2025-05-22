using System.Diagnostics;

namespace Logical.Ast.Nodes;

public class Production(Node argumentType, Node resultType, string? variableName = null, Node? annotation = null)
    : AbstractionOrProduction(resultType, variableName, argumentType, annotation)
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
}
