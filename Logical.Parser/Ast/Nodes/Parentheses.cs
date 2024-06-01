﻿namespace Logical.Parser.Ast.Nodes;

public class Parentheses(Node node, Node? annotation = null) : Node(annotation)
{
    public readonly Node Node = node;
}
