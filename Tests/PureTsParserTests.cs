using FluentAssertions;
using PureTS.Parser;
using PureTS.IR;

namespace Tests;

public sealed class PureTsParserTests
{
    // --- Literals ---

    [Fact]
    public void ParseToIr_NumberLiteral()
    {
        var term = API.parseToIR("42");
        Pretty.toString(term).Should().Be("42");
    }

    [Fact]
    public void ParseToIr_BoolTrue()
    {
        var term = API.parseToIR("true");
        Pretty.toString(term).Should().Be("true");
    }

    [Fact]
    public void ParseToIr_BoolFalse()
    {
        var term = API.parseToIR("false");
        Pretty.toString(term).Should().Be("false");
    }

    [Fact]
    public void ParseToIr_StringLiteral()
    {
        var term = API.parseToIR("\"hello world\"");
        Pretty.toString(term).Should().Be("\"hello world\"");
    }

    // --- Variables ---

    [Fact]
    public void ParseToIr_FreeVariable()
    {
        var term = API.parseToIR("x");
        Pretty.toString(term).Should().Be("x");
    }

    // --- Let ---

    [Fact]
    public void ParseToIr_LetWithNumberLiteral()
    {
        var term = API.parseToIR("let x = 1 in x");
        Pretty.toString(term).Should().Be("((λ v0) 1)");
    }

    [Fact]
    public void ParseToIr_LetWithStringLiteral()
    {
        var term = API.parseToIR("let x = \"hi\" in x");
        Pretty.toString(term).Should().Be("((λ v0) \"hi\")");
    }

    [Fact]
    public void ParseToIr_NestedLet()
    {
        var term = API.parseToIR("let x = 1 in let y = 2 in x");
        Pretty.toString(term).Should().Be("((λ ((λ v1) 2)) 1)");
    }

    // --- Lambda ---

    [Fact]
    public void ParseToIr_SingleParamLambda()
    {
        var term = API.parseToIR("\\x -> x");
        Pretty.toString(term).Should().Be("(λ v0)");
    }

    [Fact]
    public void ParseToIr_MultiParamLambda()
    {
        var term = API.parseToIR("\\x y -> x");
        Pretty.toString(term).Should().Be("(λ (λ v1))");
    }

    [Fact]
    public void ParseToIr_LambdaReturnsSecondParam()
    {
        var term = API.parseToIR("\\x y -> y");
        Pretty.toString(term).Should().Be("(λ (λ v0))");
    }

    // --- Application ---

    [Fact]
    public void ParseToIr_SimpleApplication()
    {
        var term = API.parseToIR("f x");
        Pretty.toString(term).Should().Be("(f x)");
    }

    [Fact]
    public void ParseToIr_ChainedApplication()
    {
        var term = API.parseToIR("f x y");
        Pretty.toString(term).Should().Be("((f x) y)");
    }

    [Fact]
    public void ParseToIr_ApplicationWithParens()
    {
        var term = API.parseToIR("f (g x)");
        Pretty.toString(term).Should().Be("(f (g x))");
    }

    // --- Records ---

    [Fact]
    public void ParseToIr_EmptyRecord()
    {
        var term = API.parseToIR("{}");
        Pretty.toString(term).Should().Be("{}");
    }

    [Fact]
    public void ParseToIr_SingleFieldRecord()
    {
        var term = API.parseToIR("{a = 1}");
        Pretty.toString(term).Should().Be("{a: 1}");
    }

    [Fact]
    public void ParseToIr_MultiFieldRecord()
    {
        var term = API.parseToIR("{a = 1, b = true}");
        Pretty.toString(term).Should().Be("{a: 1, b: true}");
    }

    // --- Projection ---

    [Fact]
    public void ParseToIr_RecordProjection()
    {
        var term = API.parseToIR("{a = 1, b = true}.a");
        Pretty.toString(term).Should().Be("{a: 1, b: true}.a");
    }

    [Fact]
    public void ParseToIr_ChainedProjection()
    {
        var term = API.parseToIR("{a = {b = 1}}.a.b");
        Pretty.toString(term).Should().Be("{a: {b: 1}}.a.b");
    }

    // --- Combinations ---

    [Fact]
    public void ParseToIr_LetWithLambdaAndApp()
    {
        var term = API.parseToIR("let f = \\x -> x in f 5");
        Pretty.toString(term).Should().Be("((λ (v0 5)) (λ v0))");
    }

    [Fact]
    public void ParseToIr_LambdaInRecord()
    {
        var term = API.parseToIR("{f = \\x -> x}");
        Pretty.toString(term).Should().Be("{f: (λ v0)}");
    }

    [Fact]
    public void ParseToIr_ParenthesizedExpr()
    {
        var term = API.parseToIR("(\\x -> x)");
        Pretty.toString(term).Should().Be("(λ v0)");
    }
}
