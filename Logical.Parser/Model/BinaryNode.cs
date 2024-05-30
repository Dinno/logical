namespace LogicalParser.Model;

public class BinaryNode : Node
{
    public readonly Node Left;
    public readonly Node Right;
    public readonly int Shift;

    public BinaryNode(Node left, Node right, int shift)
    {
        Left = left;
        Right = right;
        Shift = shift;
    }
}
