using Logical.Ast.Nodes;
using Snapshooter.Xunit;

namespace Tests;

public class CompilerTests
{
    [Fact]
    public void Compiler_NumberZero()
    {
        var ast = new DecimalLiteral("0");

        var result = TestUtil.CompileWithDecimals(ast);

        Snapshot.Match(result);
    }

    [Fact]
    public void Compiler_NumberOne()
    {
        var ast = new DecimalLiteral("1");

        var result = TestUtil.CompileWithDecimals(ast);

        Snapshot.Match(result);
    }

    [Fact]
    public void Compiler_NumberMinusOne()
    {
        var ast = new DecimalLiteral("-1");

        var result = TestUtil.CompileWithDecimals(ast);

        Snapshot.Match(result);
    }

    [Fact]
    public void Compiler_NumberTwo()
    {
        var ast = new DecimalLiteral("2");

        var result = TestUtil.CompileWithDecimals(ast);

        Snapshot.Match(result);
    }

    [Fact]
    public void Compiler_BoundAbstraction()
    {
        var ast = new Abstraction("a", new Variable("a"));

        var result = TestUtil.Compile(ast);

        Snapshot.Match(result);
    }

    [Fact]
    public void Compiler_UnoundAbstraction()
    {
        var ast = new Abstraction("b", new Variable("a"));

        var result = TestUtil.CompileWithA(ast);

        Snapshot.Match(result);
    }

    [Fact(Skip = "Not implemented")]
    public void Compiler_UndefinedVariable()
    {
        var ast = new Variable("a");
        Assert.Throws<NotImplementedException>(() => TestUtil.Compile(ast));
    }

    [Fact]
    public void Compiler_Application()
    {
        var ast = new Application(new Variable("a"), new Variable("a"));
        var result = TestUtil.CompileWithA(ast);
        Snapshot.Match(result);
    }

    [Fact(Skip = "Not implemented")]
    public void Compiler_Production()
    {
        var ast = new Production(new Variable("a"), new Variable("b"), "b");
        var result = TestUtil.CompileWithA(ast);
        Snapshot.Match(result);
    }

    [Fact(Skip = "Not implemented")]
    public void Compiler_Annotation()
    {
        var ast = new Variable("a", new Variable("a"));
        var result = TestUtil.CompileWithA(ast);
        Snapshot.Match(result);
    }

    [Fact]
    public void Compiler_Parentheses()
    {
        var ast = new Parentheses(new Variable("a"));
        // Parentheses is not implemented, expect exception
        Assert.Throws<NotImplementedException>(() => TestUtil.Compile(ast));
    }

    [Fact]
    public void Compiler_Pair()
    {
        var ast = new Pair(new Variable("a"), new Variable("b"));
        // Pair is not implemented, expect exception
        Assert.Throws<NotImplementedException>(() => TestUtil.Compile(ast));
    }
}