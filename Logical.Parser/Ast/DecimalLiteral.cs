namespace Logical.Ast;

public class DecimalLiteral : Node
{
    public readonly string Value;

    public DecimalLiteral(string value, Node? annotation = null) : base(annotation)
    {
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        var other = (DecimalLiteral)obj;
        return Value == other.Value;
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
