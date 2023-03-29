namespace Logical.Ast;

public abstract class Node
{
    public readonly Node? Annotation;

    protected Node(Node? annotation)
    {
        Annotation = annotation;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        var other = (Node)obj;
        return Equals(Annotation, other.Annotation);
    }
    public override int GetHashCode()
    {
        return (Annotation != null ? Annotation.GetHashCode() : 0);
    }
    public static bool operator ==(Node? left, Node? right)
    {
        return Equals(left, right);
    }
    public static bool operator !=(Node? left, Node? right)
    {
        return !Equals(left, right);
    }
}
