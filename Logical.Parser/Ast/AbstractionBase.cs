namespace Logical.Ast;

public class AbstractionBase : Node
{
    public readonly string? VariableName;
    public readonly Node Body;
    public readonly Node? Type;
    public bool IsUnbound;

    public AbstractionBase(string? variableName, Node body, Node? type = null, Node? annotation = null)
        : base(annotation)
    {
        VariableName = variableName;
        Body = body;
        Type = type;
    }
}
