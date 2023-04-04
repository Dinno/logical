namespace Logical.Ast;

public interface IVisitor
{
    void AbstractionOrProductionOut(AbstractionOrProduction node, int i, int level);
    void ApplicationOut(Application node);
    void PairOut(Pair node);
    void ParenthesesOut(Parentheses node);
    void Variable(Variable node, List<(int, int)> valueTuples);
    void DecimalLiteral(DecimalLiteral node);
}
