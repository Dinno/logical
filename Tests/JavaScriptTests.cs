using FluentAssertions;
using Logical.Parser.Ast;
using Logical.Parser.Ast.Nodes;
using Logical.Parser.Compiler;
using Snapshooter.Xunit;

namespace Tests;

public class JavaScriptTests
{
    [Fact]
    public void ConvertToJavaScript()
    {
        var ast = new Abstraction("a", new DecimalLiteral("1"));
        ast.IsUnbound.Should().Be(false);

        var initialVariables = new Dictionary<string, List<BindingInfo<int>>>
        {
            { "#Zpos", [new BindingInfo<int>(0, 0)] },
            { "#Zneg", [new BindingInfo<int>(1, 0)] },
            { "#xO", [new BindingInfo<int>(2, 0)] },
            { "#xI", [new BindingInfo<int>(3, 0)] },
            { "#xH", [new BindingInfo<int>(4, 0)] },
        };
        var compiler = new Compiler(4, initialVariables);
        var model = compiler.Compile(ast);
        
        Snapshot.Match(model.Node);
    }
}