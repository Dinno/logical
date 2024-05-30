using System.Diagnostics;

namespace LogicalParser.Ast.Nodes;

public class AbstractionOrProduction : Node
{
    public readonly string? VariableName;
    public readonly Node Body;
    public readonly Node? Type;
    private bool _isUnbound;

    public AbstractionOrProduction(Node body, string? variableName, Node? type = null, Node? annotation = null)
        : base(annotation)
    {
        VariableName = variableName;
        Body = body;
        Type = type;
    }
    public bool IsUnbound
    {
        get => _isUnbound;
        set
        {
            // We can bind only using variable name. It may change though. 
            Debug.Assert(!value && VariableName is not null || value); 
            
            _isUnbound = value;
        }
    }
}
