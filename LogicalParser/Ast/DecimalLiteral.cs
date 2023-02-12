namespace Logical.Ast;

public class DecimalLiteral : Node
{
    public readonly string Value;

    public DecimalLiteral(string value, Node? annotation) : base(annotation)
    {
        Value = value;
    }

    protected bool Equals(DecimalLiteral other)
    {
        return base.Equals(other) && Value == other.Value;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((DecimalLiteral)obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Value);
    }
    public static bool operator ==(DecimalLiteral? left, DecimalLiteral? right)
    {
        return Equals(left, right);
    }
    public static bool operator !=(DecimalLiteral? left, DecimalLiteral? right)
    {
        return !Equals(left, right);
    }
}
