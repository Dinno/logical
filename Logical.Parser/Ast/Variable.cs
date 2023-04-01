﻿namespace Logical.Ast;

public class Variable : Node
{
    public readonly string Name;

    public Variable(string name, Node? annotation = null) : base(annotation)
    {
        Name = name;
    }
}