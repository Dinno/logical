using FluentAssertions;
using Logical.Ast.Nodes;
using Snapshooter;
using Snapshooter.Xunit;

namespace Tests;

public class ParserTests
{
    public static TheoryData<string, string> CorrectSamples => new()
    {
        { "0", "Decimal literal 0" },
        { "1", "Decimal literal 1" },
        { "-1", "Decimal literal minus 1" },
        { "2", "Decimal literal 2" },
        { "-2", "Decimal literal minus 2" },
        { "10", "Decimal literal 10" },
        { "-10", "Decimal literal minus 10" },
        { "100", "Decimal literal 100" },
        { "-100", "Decimal literal minus 100" },        
        {" 1 ", "Decimal literal 1 with whitespace"},   
        { "a; b", "Abstraction with semicolon" },
        { "a = 1; a", "Application with assignment" },
        { "a => b", "Abstraction with arrow" },
        { "c = a => a; c", "Abstraction with assignment" },
        { "a b", "Application with two variables" },
        { "a b c", "Application with three variables" },
        { "a (b c)", "Application with parentheses" },
        { "a (b => c)", "Application with abstraction in parentheses" },
        { "a => b c", "Abstraction with arrow and application" },
        { "a : Type; a", "Abstraction with type" },
        { "a: Type => a", "Abstraction with type" },
        { "Type -> Type", "Type arrow type" },
        { "x : Type -> Type", "Variable with arrow type" },
        { "a: x:Type -> Type; a", "Abstraction with type dependent arrow type" },
        { "set1: (Int, Str, Float) -> Bool; set1", "Abstraction with arrow type and tuple" },
    };

    [Theory, MemberData(nameof(CorrectSamples))]
    public void Parse_CorrectProgram_CorrectAst(string program, string testSubCaseName)
    {
        var parser = new Logical.Parser.Parser();
        var ast = parser.Parse(program);
        Snapshot.Match(ast, new SnapshotNameExtension(testSubCaseName));
    }
}
