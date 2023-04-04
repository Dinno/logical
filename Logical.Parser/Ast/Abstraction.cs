using System.Diagnostics;

namespace Logical.Ast;

public class Abstraction : AbstractionOrProduction
{
    public Abstraction(Node body, string? variableName, Node? type = null, Node? annotation = null)
        : base(body, variableName, type, annotation)
    {}
}
