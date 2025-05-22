using Logical.Ast.Nodes;
using Logical.Parser;

namespace Tests;

public static class TestUtil
{
    private static Node AugmentAst(Node ast)
    {
        return new Abstraction("#binIntZero",
            new Abstraction("#binIntPos",
                new Abstraction("#binIntNeg",
                    new Abstraction("#binPosHead",
                        new Abstraction("#binPosZero",
                            new Abstraction("#binPosOne",
                                ast
                            )
                        )
                    )
                )
            )
        );
    }

    public static Logical.Model.Node Compile(Node ast)
    {
        var augmentedAst = AugmentAst(ast);
        var astBindStatesCalculator = new AstBindStatesCalculator();
        astBindStatesCalculator.Calculate(augmentedAst);
        var compiler = new Compiler();
        var model = compiler.Compile(augmentedAst);
        return model.Node;
    }
}