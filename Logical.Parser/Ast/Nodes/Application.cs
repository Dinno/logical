namespace Logical.Parser.Ast.Nodes;

public class Application : Node
{
    public readonly Node Function;
    public readonly Node Argument;

    public Application(Node function, Node argument, Node? annotation = null) : base(annotation)
    {
        Function = function;
        Argument = argument;
    }
}
