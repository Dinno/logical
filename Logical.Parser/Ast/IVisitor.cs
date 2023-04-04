namespace Logical.Ast;

public interface IVisitor
{
    /// <summary>
    /// Is called when we go out of abstraction or production node.
    /// Used when the abstraction or production potentially may be bound. 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="i"></param>
    /// <param name="level"></param>
    void AbstractionOrProductionOut(AbstractionOrProduction node, int i, int level);
    
    /// <summary>
    /// Is called when we go out of abstraction or production node.
    /// Used when we already know that the abstraction or production is unbound
    /// </summary>
    /// <param name="node">AST node</param>
    /// <param name="level">Binding level of the node</param>
    void UnboundAbstractionOrProductionOut(AbstractionOrProduction node, int level);
    void ApplicationOut(Application node);
    void PairOut(Pair node);
    void ParenthesesOut(Parentheses node);
    void Variable(Variable node, List<(int, int)> valueTuples);
    void DecimalLiteral(DecimalLiteral node);
}
