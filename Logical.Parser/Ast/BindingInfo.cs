namespace LogicalParser.Ast;

public struct BindingInfo<T>
{
    public readonly int Level;
    public T Data;

    public BindingInfo(int level, T data)
    {
        Level = level;
        Data = data;
    }
}