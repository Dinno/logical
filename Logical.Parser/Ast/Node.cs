namespace Logical.Ast;

public abstract class Node
{
    public readonly Node? Annotation;

    protected Node(Node? annotation)
    {
        Annotation = annotation;
    }
}
