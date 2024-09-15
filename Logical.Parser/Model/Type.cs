namespace Logical.Parser.Model;

public class Type(int level) : Node
{
    public int Level { get; } = level;
}
