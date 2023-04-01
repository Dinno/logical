using System.Diagnostics;

namespace Logical.Ast;

public class Abstraction : AbstractionBase
{
    public new string VariableName
    {
        get
        {
            Debug.Assert(base.VariableName is not null, "VariableName is empty!");
            return base.VariableName;
        }
    }

    public Abstraction(string variableName, Node body, Node? type = null, Node? annotation = null)
        : base(variableName, body, type, annotation)
    {}
}
