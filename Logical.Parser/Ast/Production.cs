namespace Logical.Ast;

public class Production : Node
{
    public readonly Node ArgumentType;
    public readonly Node ResultType;
    public readonly string? VariableName;


    public Production(Node argumentType, Node resultType, string? variableName = null, Node? annotation = null) : base(annotation)
    {
        ArgumentType = argumentType;
        ResultType = resultType;
        VariableName = variableName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        var other = (Production)obj;
        return ArgumentType.Equals(other.ArgumentType) && ResultType.Equals(other.ResultType) && VariableName == other.VariableName;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), ArgumentType, ResultType, VariableName);
    }
    public static bool operator ==(Production? left, Production? right)
    {
        return Equals(left, right);
    }
    public static bool operator !=(Production? left, Production? right)
    {
        return !Equals(left, right);
    }
}
