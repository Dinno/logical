using System.Diagnostics;

namespace Logical.Ast.Nodes;

public class AbstractionOrProduction(Node body, string? variableName, Node? type = null, Node? annotation = null)
    : Node(annotation)
{
    public readonly string? VariableName = variableName;
    public readonly Node Body = body;
    public readonly Node? Type = type;
    private bool _isUnbound;

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

    public override TResult Visit<TResult>(IAstVisitor<TResult> astVisitor)
    {
        return astVisitor.Visit(this);
    }
}
