namespace Logical.Ast;

public class Application : Node
{
    public Node Function;
    public Node Argument;

    public Application(Node function, Node argument, Node? annotation) : base(annotation)
    {
        Function = function;
        Argument = argument;
    }

    protected bool Equals(Application other)
    {
        return base.Equals(other) && Function.Equals(other.Function) && Argument.Equals(other.Argument);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((Application)obj);
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
