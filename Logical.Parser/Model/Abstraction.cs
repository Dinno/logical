namespace Logical.Model;

public class Abstraction : Node
{
    public readonly Node Body;

    public Abstraction(Node body)
    {
        Body = body;
    }
}
