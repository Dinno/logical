namespace Logical.Ast;

public class Pair : Node
{
    public readonly Node Left;
    public readonly Node Right;

    public Pair(Node left, Node right, Node? annotation = null) : base(annotation)
    {
        Left = left;
        Right = right;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        var other = (Pair)obj;
        return Left.Equals(other.Left) && Right.Equals(other.Right);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Left, Right);
    }
    public static bool operator ==(Pair? left, Pair? right)
    {
        return Equals(left, right);
    }
    public static bool operator !=(Pair? left, Pair? right)
    {
        return !Equals(left, right);
    }
}
