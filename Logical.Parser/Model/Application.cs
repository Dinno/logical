namespace Logical.Parser.Model;

public class Application : BinaryNode
{
    public Application(Node function, Node argument, int shift) : base(function, argument, shift)
    {
    }
}
