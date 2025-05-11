using FluentAssertions;
using Logical.Parser.Ast;
using Logical.Parser.Ast.Nodes;
using Logical.Parser.Compiler;
using Snapshooter.Xunit;

namespace Tests;

public class CompilerTests
{
    private Dictionary<string, List<BindingInfo<int>>> _initialVariables;
    private int _initialDepth;

    public CompilerTests()
    {
        _initialVariables = new Dictionary<string, List<BindingInfo<int>>>
        {
            { "#Zpos", [new BindingInfo<int>(0, 0)] },
            { "#Zneg", [new BindingInfo<int>(1, 0)] },
            { "#xO", [new BindingInfo<int>(2, 0)] },
            { "#xI", [new BindingInfo<int>(3, 0)] },
            { "#xH", [new BindingInfo<int>(4, 0)] },
        };
        _initialDepth = 4;
    }

    [Fact]
    public void Compiler_AbstractionWithNumber()
    {
        var ast = new Abstraction("a", new DecimalLiteral("1"));
        ast.IsUnbound.Should().Be(false);
        var compiler = new Compiler(_initialDepth, _initialVariables);
        var model = compiler.Compile(ast);

        Snapshot.Match(model.Node);
    }

    [Fact]
    public void Compiler_AbstractionWithVar()
    {
        var ast = new Abstraction("a", new Variable("a"));
        var compiler = new Compiler(_initialDepth, _initialVariables);
        var model = compiler.Compile(ast);

        Snapshot.Match(model.Node);
    }

    // [Fact]
    // public void Compiler_Variable()
    // {
    //     var ast = new Variable("a");
    //     var compiler = new Compiler(_initialDepth, _initialVariables);
    //     var model = compiler.Compile(ast);
    //     Snapshot.Match(model.Node);
    // }

    // [Fact]
    // public void Compiler_Application()
    // {
    //     var ast = new Application(new Variable("f"), new Variable("x"));
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