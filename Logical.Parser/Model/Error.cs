namespace Logical.Parser.Model;

public class Error : Node
{
    public readonly int Index;
    
    public Error(int index)
    {
        Index = index;
    }
}
