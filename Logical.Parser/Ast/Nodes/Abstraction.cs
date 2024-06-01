namespace Logical.Parser.Ast.Nodes;

public class Abstraction(string? variableName, Node body, Node? type = null, Node? annotation = null)
    : AbstractionOrProduction(body, variableName, type, annotation);
