namespace Logical.Model;

public class BinaryNode(Node left, Node right, int shift) : Node
{
    public Node Left { get; } = left;
    public Node Right { get; } = right;
    public int Shift { get; } = shift;
}
