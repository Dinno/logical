namespace Logical.Ast;

public class Variable : Node
{
    public readonly string Name;

    public Variable(string name, Node? annotation) : base(annotation)
    {
        Name = name;
    }

    protected bool Equals(Variable other)
    {
        return base.Equals(other) && Name == other.Name;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((Variable)obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Name);
    }
    public static bool operator ==(Variable? left, Variable? right)
    {
        return Equals(left, right);
    }
    public static bool operator !=(Variable? left, Variable? right)
    {
        return !Equals(left, right);
    }
}
