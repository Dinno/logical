using Logical.Ast;
using Logical.Model;
using Abstraction = Logical.Ast.Abstraction;
using Application = Logical.Ast.Application;
using Node = Logical.Model.Node;
using Production = Logical.Ast.Production;
using Variable = Logical.Model.Variable;

namespace Logical.Compiler;

public class Compiler
{
    private int _level = 0;
    
    /// <summary>
    /// Is used to count variable references and to store variable depths 
    /// </summary>
    private Dictionary<string, List<int>> _variables = new Dictionary<string, List<int>>();

    // public Node Compile(Ast.Node ast)
    // {
    //     return CompileRec(ast);
    // }
    //
    // private (Node, int MinRef) CompileRec(Ast.Node ast)
    // {
    //     var modelNode = ast switch
    //     {
    //         Ast.Abstraction abstraction => BuildBinaryNode<Annotation>(CompileRec(abstraction.)),
    //         Ast.Application application => throw new NotImplementedException(),
    //         Ast.DecimalLiteral decimalLiteral => throw new NotImplementedException(),
    //         Ast.Pair pair => throw new NotImplementedException(),
    //         Ast.Parentheses parentheses => throw new NotImplementedException(),
    //         Ast.Production production => throw new NotImplementedException(),
    //         Ast.Variable variable => new Variable(),
    //         _ => throw new ArgumentOutOfRangeException(nameof(ast))
    //
    //     };
    //     return modelNode;
    // }

    // public void PrepareAst(Ast.Node ast)
    // {
    //     PrepareAstRec(ast);
    // }
    // private void PrepareAstRec(Ast.Node ast)
    // {
    //     switch (ast)
    //     {
    //         case Production production:
    //         case Abstraction abstraction:
    //             List<int> varList;
    //             var exists = _variables.TryGetValue(abstraction.VariableName, out varList);
    //             if (!exists)
    //             {
    //                 varList = new List<int>();
    //                 _variables.Add(abstraction.VariableName, varList);
    //             }
    //             var varIndex = varList.Count;
    //             varList.Add(_level++);
    //             
    //             
    //             
    //             varList.RemoveAt(varIndex);
    //             _level--;
    //             break;
    //         case Application application:
    //             break;
    //         case DecimalLiteral decimalLiteral:
    //             break;
    //         case Pair pair:
    //             break;
    //         case Parentheses parentheses:
    //             break;
    //             break;
    //         case Ast.Variable variable:
    //             break;
    //         default:
    //             throw new ArgumentOutOfRangeException(nameof(ast));
    //     }
    // }
}
