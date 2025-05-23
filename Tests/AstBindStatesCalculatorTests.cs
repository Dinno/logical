﻿using FluentAssertions;
using Logical.Ast.Nodes;
using Logical.Parser;

namespace Tests;

public class AstBindStatesCalculatorTests
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
        var ast = new Abstraction("a", new Variable("b"));
        ast.IsUnbound.Should().Be(false);
        
        var astBindStatesCalculator = new AstBindStatesCalculator();
        astBindStatesCalculator.Calculate(ast);
        ast.IsUnbound.Should().Be(true);
    }
}
