namespace Logical.Ast;

public class Application : Node
{
    public readonly Node Function;
    public readonly Node Argument;

    public Application(Node function, Node argument, Node? annotation = null) : base(annotation)
    {
        Function = function;
        Argument = argument;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        var other = (Application)obj;
        return Function.Equals(other.Function) && Argument.Equals(other.Argument);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Function, Argument);
    }
    public static bool operator ==(Application? left, Application? right)
    {
        return Equals(left, right);
    }
    public static bool operator !=(Application? left, Application? right)
    {
        return !Equals(left, right);
    }
}
