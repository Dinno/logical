namespace Logical.Ast;

public class Abstraction : Node
{
    public readonly string VariableName;
    public readonly Node Body;

    public Abstraction(string variableName, Node body, Node? annotation)
        : base(annotation)
    {
        VariableName = variableName;
        Body = body;
    }

    protected bool Equals(Abstraction other)
    {
        return base.Equals(other) && VariableName == other.VariableName && Body.Equals(other.Body);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((Abstraction)obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), VariableName, Body);
    }
    public static bool operator ==(Abstraction? left, Abstraction? right)
    {
        return Equals(left, right);
    }
    public static bool operator !=(Abstraction? left, Abstraction? right)
    {
        return !Equals(left, right);
    }
}
