namespace Logical.Parser.Ast.Nodes;

public class Pair(Node left, Node right, Node? annotation = null) : Node(annotation)
{
    public readonly Node Left = left;
    public readonly Node Right = right;
}
