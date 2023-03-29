namespace Logical.Ast;

public class Variable : Node
{
    public readonly string Name;

    public Variable(string name, Node? annotation = null) : base(annotation)
    {
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        var other = (Variable)obj;
        return Name == other.Name;
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
