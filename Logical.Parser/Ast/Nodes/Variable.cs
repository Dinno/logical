namespace Logical.Parser.Ast.Nodes;

public class Variable(string name, Node? annotation = null) : Node(annotation)
{
    public readonly string Name = name;
    
    public override TResult Visit<TResult>(IAstVisitor<TResult> astVisitor)
    {
        return astVisitor.Visit(this);
    }
}
