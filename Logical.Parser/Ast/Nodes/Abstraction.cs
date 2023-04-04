﻿using System.Diagnostics;

namespace Logical.Ast;

public class Abstraction : AbstractionOrProduction
{
    public Abstraction(string? variableName, Node body, Node? type = null, Node? annotation = null)
        : base(body, variableName, type, annotation)
    {}
}