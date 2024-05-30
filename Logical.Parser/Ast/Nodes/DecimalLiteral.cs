namespace LogicalParser.Ast.Nodes;

public class DecimalLiteral : Node
{
    public readonly string Value;

    public DecimalLiteral(string value, Node? annotation = null) : base(annotation)
    {
        Value = value;
    }
}
