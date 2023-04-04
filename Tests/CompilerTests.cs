using FluentAssertions;
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
        
        var compiler = new Compiler();
        compiler.CalcAstBindStates(ast);
        ast.IsUnbound.Should().Be(false);
    }
    
    [Fact]
    public void CalcAstBindStates_Unbound()
    {
        var ast = new Abstraction("a", new DecimalLiteral("1"));
        ast.IsUnbound.Should().Be(false);
        
        var compiler = new Compiler();
        compiler.CalcAstBindStates(ast);
        ast.IsUnbound.Should().Be(true);
    }
}
