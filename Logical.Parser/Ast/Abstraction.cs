namespace Logical.Ast;

public class Abstraction : Node
{
    public readonly string VariableName;
    public readonly Node Body;
    public readonly Node? Type;
    public bool IsUnbound;

    public Abstraction(string variableName, Node body, Node? type = null, Node? annotation = null)
        : base(annotation)
    {
        VariableName = variableName;
        Body = body;
        Type = type;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        var other = (Abstraction)obj;
        return VariableName == other.VariableName && Body.Equals(other.Body) && Equals(Type, other.Type);
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
