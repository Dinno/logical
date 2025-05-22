namespace Logical.Ast.Nodes;

public class Pair(Node left, Node right, Node? annotation = null) : Node(annotation)
{
    public readonly Node Left = left;
    public readonly Node Right = right;
    
    public override TResult Visit<TResult>(IAstVisitor<TResult> astVisitor)
    {
        return astVisitor.Visit(this);
    }
}
