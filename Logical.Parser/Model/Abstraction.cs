namespace Logical.Parser.Model;

public abstract class Abstraction(Node body) : Node
{
    public readonly Node Body = body;
}
