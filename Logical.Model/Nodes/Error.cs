namespace Logical.Model;

public class Error(int index) : Node
{
    public int Index { get; } = index;
}
