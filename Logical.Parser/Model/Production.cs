namespace Logical.Model;

public class Production : BinaryNode
{
    public Production(Node left, Node right, int shift) : base(left, right, shift)
    {
    }
}
