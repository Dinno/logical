namespace Logical.Ast;

public abstract class Node : IEquatable<Node>
{
    public readonly Node? Annotation;

    protected Node(Node? annotation)
    {
        this.Annotation = annotation;
    }

    public bool Equals(Node? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Equals(Annotation, other.Annotation);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((Node)obj);
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
