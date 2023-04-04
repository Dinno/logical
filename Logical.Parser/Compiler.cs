using Logical.Ast;
using Application = Logical.Ast.Application;

namespace Logical.Compiler;

public class Compiler
{
    private int _level = 0;

    /// <summary>
    /// Is used to count variable references and to store variable depths 
    /// </summary>
    private readonly Dictionary<string, List<(int, int)>> _variables = new();

    // public Node Compile(Ast.Node ast)
    // {
    //     return CompileRec(ast);
    // }

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

    public void CalcAstBindStates(Node ast)
    {
        CalcAstBindStatesRec(ast);
    }
    private void CalcAstBindStatesRec(Node ast)
    {
        while (true)
        {
            switch (ast)
            {
                case AbstractionOrProduction abstractionOrProduction:
                {
                    if (abstractionOrProduction.Type is not null)
                        CalcAstBindStatesRec(abstractionOrProduction.Type);

                    if (abstractionOrProduction.Annotation is not null)
                        CalcAstBindStatesRec(abstractionOrProduction.Annotation);

                    var variableName = abstractionOrProduction.VariableName;
                    if (abstractionOrProduction.IsUnbound || variableName is null)
                    {
                        CalcAstBindStatesRec(abstractionOrProduction.Body);
                        abstractionOrProduction.IsUnbound = true;
                    }

                    var exists = _variables.TryGetValue(variableName, out var varList);
                    if (!exists)
                    {
                        varList = new List<(int, int)>();
                        _variables.Add(variableName, varList);
                    }
                    var varIndex = varList!.Count;
                    varList.Add((_level, 0));
                    _level++;

                    CalcAstBindStatesRec(abstractionOrProduction.Body);

                    _level--;
                    abstractionOrProduction.IsUnbound = varList[varIndex].Item2 == 0;
                    varList.RemoveAt(varIndex);

                    break;
                }
                case Application application:
                    CalcAstBindStatesRec(application.Function);
                    CalcAstBindStatesRec(application.Argument);
                    CalcAstBindStatesRec(application.Annotation);
                    break;
                case Pair pair:
                    CalcAstBindStatesRec(pair.Left);
                    CalcAstBindStatesRec(pair.Right);
                    CalcAstBindStatesRec(pair.Annotation);
                    break;
                case Parentheses parentheses:
                    CalcAstBindStatesRec(parentheses.Node);
                    CalcAstBindStatesRec(parentheses.Annotation);
                    break;
                case Variable variable:
                {
                    var varList = _variables[variable.Name];
                    var lastOccurence = varList[^1];
                    lastOccurence.Item2++;
                    varList[^1] = lastOccurence;
                    break;
                }
                case DecimalLiteral decimalLiteral:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ast));
            }
            break;
        }
    }
}
