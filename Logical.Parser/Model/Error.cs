namespace Logical.Parser.Model;

public class Error(int index) : Node
{
    public readonly int Index = index;
}
