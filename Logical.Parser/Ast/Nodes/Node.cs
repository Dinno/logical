namespace Logical.Parser.Ast.Nodes;

public abstract class Node(Node? annotation)
{
    public readonly Node? Annotation = annotation;
}
