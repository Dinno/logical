using FluentAssertions;
using Logical.Ast;

namespace ParserTests;

public class ParserTests
{
    public static object[][] CorrectSamples =
    {
        new object[]
        {
            "1", new DecimalLiteral("1")
        },
        new object[]
        {
            " 1 ", new DecimalLiteral("1")
        },
        new object[]
        {
            "a; b", new Abstraction("a", new Variable("b"))
        },
        new object[]
        {
            "a = 1; a", new Application(new Abstraction("a", new Variable("a")), new DecimalLiteral("1"))
        },
        new object[]
        {
            "a => b", new Abstraction("a", new Variable("b"))
        },
        new object[]
        {
            "c = a => a; c",
            new Application(
                new Abstraction("c", new Variable("c")),
                new Abstraction("a", new Variable("a")))
        },
        new object[]
        {
            "a b", new Application(new Variable("a"), new Variable("b"))
        },
        new object[]
        {
            "a b c", new Application(new Application(new Variable("a"), new Variable("b")), new Variable("c"))
        },
        new object[]
        {
            "a (b c)", new Application(new Variable("a"), new Parentheses(new Application(new Variable("b"), new Variable("c"))))
        },
        new object[]
        {
            "a (b => c)", new Application(new Variable("a"), new Parentheses(new Abstraction("b", new Variable("c"))))
        },
        new object[]
        {
            "a => b c", new Abstraction("a", new Application(new Variable("b"), new Variable("c")))
        },
        new object[]
        {
            "a : Type; a", new Abstraction("a", new Variable("a"), new Variable("Type"))
        },
        new object[]
        {
            "a: Type => a", new Abstraction("a", new Variable("a"), new Variable("Type"))
        },
        new object[]
        {
            "Type -> Type", new Production(new Variable("Type"), new Variable("Type"))
        },
        new object[]
        {
            "x : Type -> Type", new Production(new Variable("Type"), new Variable("Type"), "x")
        },
        new object[]
        {
            "a: x:Type -> Type; a", new Abstraction("a", new Variable("a"), new Production(new Variable("Type"), new Variable("Type"), "x"))
        },
        new object[]
        {
            "set1: (Int, Str, Float) -> Bool; set1",
            new Abstraction("set1", new Variable("set1"),
                new Production(new Parentheses(new Pair(new Variable("Int"), new Pair(new Variable("Str"), new Variable("Float")))),
                    new Variable("Bool")))
        },
    };

    [Theory, MemberData(nameof(CorrectSamples))]
    public void Parse_CorrectProgram_CorrectAst(string program, Node astReference)
    {
        var parser = new Logical.Parser();
        var ast = parser.Parse(program);
        ast.Should().BeEquivalentTo(astReference, options => options.RespectingRuntimeTypes());
    }
}
