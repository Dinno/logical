using Logical.Ast.Nodes;
using Snapshooter.Xunit;

namespace Tests;

public class CompilerTests
{
    [Fact]
    public void Compiler_NumberZero()
    {
        var ast = new DecimalLiteral("0");

        var result = TestUtil.Compile(ast);

        Snapshot.Match(result);
    }

    [Fact]
    public void Compiler_NumberOne()
    {
        var ast = new DecimalLiteral("1");
        
        var result = TestUtil.Compile(ast);

        Snapshot.Match(result);
    }

    [Fact]
    public void Compiler_NumberMinusOne()
    {
        var ast = new DecimalLiteral("-1");
        
        var result = TestUtil.Compile(ast);

        Snapshot.Match(result);
    }

    [Fact]
    public void Compiler_NumberTwo()
    {
        var ast = new DecimalLiteral("2");
        
        var result = TestUtil.Compile(ast);

        Snapshot.Match(result);
    }

    // [Fact]
    // public void Compiler_AbstractionWithNumberOne()
    // {
    //     var ast = new Abstraction("a", new DecimalLiteral("1"));
    //     ast.IsUnbound.Should().Be(false);
    //     var compiler = new Compiler(_initialDepth, _initialVariables);
    //     var model = compiler.Compile(ast);

    //     Snapshot.Match(model.Node);
    // }

    // [Fact]
    // public void Compiler_AbstractionWithNumberTwo()
    // {
    //     var ast = new Abstraction("a", new DecimalLiteral("2"));
    //     ast.IsUnbound.Should().Be(false);
    //     var compiler = new Compiler(_initialDepth, _initialVariables);
    //     var model = compiler.Compile(ast);

    //     Snapshot.Match(model.Node);
    // }

    // [Fact]
    // public void Compiler_AbstractionWithVar()
    // {
    //     var ast = new Abstraction("a", new Variable("a"));
    //     var compiler = new Compiler(_initialDepth, _initialVariables);
    //     var model = compiler.Compile(ast);

    //     Snapshot.Match(model.Node);
    // }

    // [Fact]
    // public void Compiler_Variable()
    // {
    //     var ast = new Variable("a");
    //     var compiler = new Compiler(_initialDepth, _initialVariables);
    //     Assert.Throws<KeyNotFoundException>(() => compiler.Compile(ast));
    // }

    // [Fact]
    // public void Compiler_Application()
    // {
    //     var ast = new Application(new Variable("#Zpos"), new Variable("#Zpos"));
    //     var compiler = new Compiler(_initialDepth, _initialVariables);
    //     var model = compiler.Compile(ast);
    //     Snapshot.Match(model.Node);
    // }

    // [Fact]
    // public void Compiler_Production()
    // {
    //     var ast = new Production(new Variable("A"), new Variable("B"), "x");
    //     var compiler = new Compiler(_initialDepth, _initialVariables);
    //     var model = compiler.Compile(ast);
    //     Snapshot.Match(model.Node);
    // }

    // [Fact]
    // public void Compiler_Annotation()
    // {
    //     var ast = new Variable("a", new Variable("ann"));
    //     var compiler = new Compiler(_initialDepth, _initialVariables);
    //     var model = compiler.Compile(ast);
    //     Snapshot.Match(model.Node);
    // }

    // [Fact]
    // public void Compiler_DecimalLiteral()
    // {
    //     var ast = new DecimalLiteral("42");
    //     var compiler = new Compiler(_initialDepth, _initialVariables);
    //     var model = compiler.Compile(ast);
    //     Snapshot.Match(model.Node);
    // }

    // [Fact]
    // public void Compiler_Parentheses()
    // {
    //     var ast = new Parentheses(new Variable("a"));
    //     var compiler = new Compiler(_initialDepth, _initialVariables);
    //     // Parentheses is not implemented, expect exception
    //     Assert.Throws<NotImplementedException>(() => compiler.Compile(ast));
    // }

    // [Fact]
    // public void Compiler_Pair()
    // {
    //     var ast = new Pair(new Variable("a"), new Variable("b"));
    //     var compiler = new Compiler(_initialDepth, _initialVariables);
    //     // Pair is not implemented, expect exception
    //     Assert.Throws<NotImplementedException>(() => compiler.Compile(ast));
    // }
}