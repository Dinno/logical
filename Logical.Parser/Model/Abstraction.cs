namespace Logical.Parser.Model;

public abstract class Abstraction(Node body) : Node
{
    public Node Body { get; } = body;
}