namespace Logical.Parser.Ast.Nodes;

public class DecimalLiteral(string value, Node? annotation = null) : Node(annotation)
{
    public readonly string Value = value;
    
    public override TResult Visit<TResult>(IAstVisitor<TResult> astVisitor)
    {
        return astVisitor.Visit(this);
    }
}
