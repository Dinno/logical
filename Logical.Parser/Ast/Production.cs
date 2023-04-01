using System.Diagnostics;

namespace Logical.Ast;

public class Production : AbstractionBase
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
        base(variableName, argumentType annotation)
    {
        ArgumentType = argumentType;
        ResultType = resultType;
        VariableName = variableName;
    }
}
