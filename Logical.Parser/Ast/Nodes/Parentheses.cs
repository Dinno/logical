namespace LogicalParser.Ast.Nodes;

public class Parentheses : Node
{
    public readonly Node Node;

    public Parentheses(Node node, Node? annotation = null) : base(annotation)
    {
        Node = node;
    }
}
