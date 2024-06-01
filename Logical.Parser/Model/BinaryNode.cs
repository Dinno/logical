namespace Logical.Parser.Model;

public class BinaryNode(Node left, Node right, int shift) : Node
{
    public readonly Node Left = left;
    public readonly Node Right = right;
    public readonly int Shift = shift;
}
