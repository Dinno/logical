namespace Logical.Ast;

public class AstTraversal
{
    private int _level = 0;

    /// <summary>
    /// Is used to count variable references and to store variable depths 
    /// </summary>
    private readonly Dictionary<string, List<(int, int)>> _variables = new();
    private readonly IVisitor _visitor;

    public AstTraversal(IVisitor visitor)
    {
        _visitor = visitor;
    }

    public void Traverse(Node ast)
    {
        TraverseRec(ast);
    }

    private void TraverseRec(Node ast)
    {
        while (true)
        {
            switch (ast)
            {
                case AbstractionOrProduction abstractionOrProduction:
                {
                    if (abstractionOrProduction.Type is not null)
                        TraverseRec(abstractionOrProduction.Type);

                    if (abstractionOrProduction.Annotation is not null)
                        TraverseRec(abstractionOrProduction.Annotation);

                    var variableName = abstractionOrProduction.VariableName;
                    if (abstractionOrProduction.IsUnbound || variableName is null)
                    {
                        TraverseRec(abstractionOrProduction.Body);
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

                    TraverseRec(abstractionOrProduction.Body);

                    _level--;
                    abstractionOrProduction.IsUnbound = varList[varIndex].Item2 == 0;
                    varList.RemoveAt(varIndex);

                    _visitor.AbstractionOrProductionOut(abstractionOrProduction, _level, varIndex);
                    break;
                }
                case Application application:
                    TraverseRec(application.Function);
                    TraverseRec(application.Argument);
                    if (application.Annotation is not null)
                        TraverseRec(application.Annotation);
                    _visitor.ApplicationOut(application);
                    break;
                case Pair pair:
                    TraverseRec(pair.Left);
                    TraverseRec(pair.Right);
                    if (pair.Annotation is not null)
                        TraverseRec(pair.Annotation);
                    _visitor.PairOut(pair);
                    break;
                case Parentheses parentheses:
                    TraverseRec(parentheses.Node);
                    if (parentheses.Annotation is not null)
                        TraverseRec(parentheses.Annotation);
                    _visitor.ParenthesesOut(parentheses);
                    break;
                case Variable variable:
                {
                    var varList = _variables[variable.Name];
                    _visitor.Variable(variable, varList);
                    break;
                }
                case DecimalLiteral decimalLiteral:
                    _visitor.DecimalLiteral(decimalLiteral);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ast));
            }
            break;
        }
    }
}
