using Logical.Ast;
using Logical;

namespace ParserTests;

public class ParserTests
{
    public static object[][] CorrectSamples =
    {
        new object[]
        {
            "1", new DecimalLiteral("1", null)
        },
        new object[]
        {
            "a; b", new DecimalLiteral("1", null)
        }
    };

    [Theory, MemberData(nameof(CorrectSamples))]
    public void Parse_CorrectProgram_CorrectAst(string program, Node astReference)
    {
        var parser = new Parser();
        var ast = parser.Parse(program);
        Assert.Equal(astReference, ast);
    }
}
