using FluentAssertions;
using PureTS.Parser;
using PureTS.VM;

namespace Tests;

public sealed class PureTsVmTests
{
    private static string Eval(string input) =>
        Pretty.valueToString(PureTS.VM.Eval.run(API.parseToIR(input)));

    // --- Literals ---

    [Fact]
    public void Run_NumberLiteral()
    {
        Eval("1").Should().Be("1");
    }

    [Fact]
    public void Run_LargeNumber()
    {
        Eval("9999").Should().Be("9999");
    }

    [Fact]
    public void Run_BoolTrue()
    {
        Eval("true").Should().Be("true");
    }

    [Fact]
    public void Run_BoolFalse()
    {
        Eval("false").Should().Be("false");
    }

    [Fact]
    public void Run_StringLiteral()
    {
        Eval("\"hello\"").Should().Be("\"hello\"");
    }

    // --- Let ---

    [Fact]
    public void Run_LetBoundVariable()
    {
        Eval("let x = 2 in x").Should().Be("2");
    }

    [Fact]
    public void Run_NestedLet()
    {
        Eval("let x = 1 in let y = 2 in y").Should().Be("2");
    }

    [Fact]
    public void Run_NestedLetShadowing()
    {
        Eval("let x = 1 in let x = 2 in x").Should().Be("2");
    }

    [Fact]
    public void Run_LetOuterVariable()
    {
        Eval("let x = 1 in let y = 2 in x").Should().Be("1");
    }

    // --- Lambda & Application ---

    [Fact]
    public void Run_IdentityFunction()
    {
        Eval("let f = \\x -> x in f 5").Should().Be("5");
    }

    [Fact]
    public void Run_ConstFunction()
    {
        Eval("let k = \\x y -> x in k 1 2").Should().Be("1");
    }

    [Fact]
    public void Run_FlipFunction()
    {
        Eval("let k = \\x y -> y in k 1 2").Should().Be("2");
    }

    [Fact]
    public void Run_LambdaReturnsLambda()
    {
        Eval("let f = \\x -> \\y -> x in f 3 4").Should().Be("3");
    }

    [Fact]
    public void Run_ClosureCapture()
    {
        Eval("let x = 10 in let f = \\y -> x in f 0").Should().Be("10");
    }

    // --- Records ---

    [Fact]
    public void Run_EmptyRecord()
    {
        Eval("{}").Should().Be("{}");
    }

    [Fact]
    public void Run_SingleFieldRecord()
    {
        Eval("{a = 1}").Should().Be("{a: 1}");
    }

    [Fact]
    public void Run_MultiFieldRecord()
    {
        Eval("{a = 1, b = true}").Should().Be("{a: 1, b: true}");
    }

    // --- Projection ---

    [Fact]
    public void Run_ProjectFirstField()
    {
        Eval("{a = 1, b = 2}.a").Should().Be("1");
    }

    [Fact]
    public void Run_ProjectSecondField()
    {
        Eval("{a = 1, b = \"hi\"}.b").Should().Be("\"hi\"");
    }

    [Fact]
    public void Run_NestedRecordProjection()
    {
        Eval("{a = {b = 42}}.a.b").Should().Be("42");
    }

    // --- Combinations ---

    [Fact]
    public void Run_LetWithRecordProjection()
    {
        Eval("let r = {x = 10, y = 20} in r.y").Should().Be("20");
    }

    [Fact]
    public void Run_LambdaOverRecord()
    {
        Eval("let getX = \\r -> r.x in getX {x = 7}").Should().Be("7");
    }

    [Fact]
    public void Run_RecordWithLambdaField()
    {
        Eval("let r = {f = \\x -> x} in r.f 99").Should().Be("99");
    }

    [Fact]
    public void Run_FreeVariable()
    {
        Eval("x").Should().Be("x");
    }

    // --- Error cases ---

    [Fact]
    public void Run_MissingFieldProjection_Throws()
    {
        var act = () => Eval("{a = 1}.b");
        act.Should().Throw<RuntimeError>();
    }

    [Fact]
    public void Run_ProjectionOnNonRecord_Throws()
    {
        var act = () => Eval("let x = 1 in x.a");
        act.Should().Throw<RuntimeError>();
    }

    [Fact]
    public void Run_ApplyNonFunction_Throws()
    {
        var act = () => Eval("let x = 1 in x 2");
        act.Should().Throw<RuntimeError>();
    }
}
