namespace Logical.Parser.Ast.Nodes;

public class Application(Node function, Node argument, Node? annotation = null) : Node(annotation)
{
    public readonly Node Function = function;
    public readonly Node Argument = argument;
    
    public override TResult Visit<TResult>(IAstVisitor<TResult> astVisitor)
    {
        return astVisitor.Visit(this);
    }
}
