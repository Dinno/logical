namespace Logical.Ast;

public struct BindingInfo<T>
{
    public readonly int Level;
    public T Data;

    public BindingInfo(int level, T data)
    {
        Level = level;
        Data = data;
    }
}
/// <summary>
/// Implements traversal of AST using visitor pattern
/// </summary>
/// <remarks>There are two cases of AST traversal. Which case it is, depends on calculation
/// of binding status of abstractions and productions. When binding states are already calculated
/// number of binding levels may be smaller then when they are not</remarks>
public class AstTraversal<TBinding, TNode, TAstVisitor>
    where TAstVisitor : IAstVisitor<TBinding, TNode>
{
    private int _level;

    /// <summary>
    /// Is used to count variable references and to store variable depths 
    /// </summary>
    private readonly Dictionary<string, List<BindingInfo<TBinding>>> _variables = new();
    private readonly TAstVisitor _astVisitor;

    public AstTraversal(TAstVisitor astVisitor)
    {
        _astVisitor = astVisitor;
    }

    public TNode Traverse(Node ast)
    {
        return TraverseRec(ast);
    }

    private TNode TraverseRec(Node ast)
    {
        while (true)
        {
            switch (ast)
            {
                case AbstractionOrProduction abstractionOrProduction:
                {
                    TNode? type = default; 
                    if (abstractionOrProduction.Type is not null)
                        type = TraverseRec(abstractionOrProduction.Type);

                    TNode? annotation = default;
                    if (abstractionOrProduction.Annotation is not null)
                        annotation = TraverseRec(abstractionOrProduction.Annotation);

                    var variableName = abstractionOrProduction.VariableName;

                    if (abstractionOrProduction.IsUnbound || variableName is null)
                    {
                        var body = TraverseRec(abstractionOrProduction.Body);
                        return _astVisitor.UnboundAbstractionOrProductionOut(abstractionOrProduction, _level, body, type, annotation);
                    }

                    if (!_variables.TryGetValue(variableName, out var varList))
                    {
                        varList = new List<BindingInfo<TBinding>>();
                        _variables.Add(variableName, varList);
                    }
                    var varIndex = varList.Count;
                    varList.Add(new BindingInfo<TBinding>(_level, _astVisitor.CreateBindingData()));
                    _level++;

                    var body2 = TraverseRec(abstractionOrProduction.Body);

                    _level--;
                    var bindingInfo = varList[varIndex];
                    varList.RemoveAt(varIndex);

                    return _astVisitor.AbstractionOrProductionOut(abstractionOrProduction, bindingInfo, body2, type, annotation);
                }
                case Application application:
                {
                    var function = TraverseRec(application.Function);
                    var argument = TraverseRec(application.Argument);
                    TNode? annotation = default;
                    if (application.Annotation is not null)
                        TraverseRec(application.Annotation);
                    return _astVisitor.ApplicationOut(application, function, argument, annotation);
                }
                case Pair pair:
                {
                    var left = TraverseRec(pair.Left);
                    var right = TraverseRec(pair.Right);
                    if (pair.Annotation is not null)
                        TraverseRec(pair.Annotation);
                    return _astVisitor.PairOut(pair, left, right);
                }
                case Parentheses parentheses:
                {
                    var intern = TraverseRec(parentheses.Node);
                    if (parentheses.Annotation is not null)
                        TraverseRec(parentheses.Annotation);
                    return _astVisitor.ParenthesesOut(parentheses, intern);
                }
                case Variable variable:
                {
                    var varList = _variables[variable.Name];
                    return _astVisitor.Variable(variable, varList);
                }
                case DecimalLiteral decimalLiteral:
                {
                    return _astVisitor.DecimalLiteral(decimalLiteral);
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(ast));
            }
            break;
        }
    }
}
