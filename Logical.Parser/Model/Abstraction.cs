namespace LogicalParser.Model;

public abstract class Abstraction : Node
{
    public readonly Node Body;

    public Abstraction(Node body)
    {
        Body = body;
    }
}
