﻿using FluentAssertions;
using Logical.Ast;
using Logical.Compiler;

namespace Tests;

public class CompilerTests
{
    [Fact]
    public void CalcAstBindStates_Bound()
    {
        var ast = new Abstraction("a", new Variable("a"));
        ast.IsUnbound.Should().Be(false);
        
        var astBindStatesCalculator = new AstBindStatesCalculator();
        astBindStatesCalculator.Calculate(ast);
        ast.IsUnbound.Should().Be(false);
    }
    
    [Fact]
    public void CalcAstBindStates_Unbound()
    {
        var ast = new Abstraction("a", new DecimalLiteral("1"));
        ast.IsUnbound.Should().Be(false);
        
        var astBindStatesCalculator = new AstBindStatesCalculator();
        astBindStatesCalculator.Calculate(ast);
        ast.IsUnbound.Should().Be(true);
    }
    
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
