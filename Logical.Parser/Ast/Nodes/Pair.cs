namespace LogicalParser.Ast.Nodes;

public class Pair : Node
{
    public readonly Node Left;
    public readonly Node Right;

    public Pair(Node left, Node right, Node? annotation = null) : base(annotation)
    {
        Left = left;
        Right = right;
    }
}
