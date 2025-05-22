namespace Logical.Ast;

public struct BindingInfo<T>(int level, T data)
{
    public readonly int Level = level;
    public T Data = data;
}