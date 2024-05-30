﻿using FluentAssertions;
using LogicalParser.Ast.Nodes;
using LogicalParser.Compiler;

namespace Tests;

public class CompilerTests
{
    [Fact]
    public void Compiler()
    {
        var ast = new Abstraction("a", new DecimalLiteral("1"));
        ast.IsUnbound.Should().Be(false);
        
        var compiler = new Compiler();
        var model = compiler.Compile(ast);
        model.Should().NotBeNull();
    }
}