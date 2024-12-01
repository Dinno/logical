namespace Logical.Parser.Ast;

public interface IAstVisitor<out TResult>
    where TResult : struct
{
    TResult Visit(Nodes.AbstractionOrProduction node);
    TResult Visit(Nodes.Application node);
    TResult Visit(Nodes.Pair node);
    TResult Visit(Nodes.Parentheses node);
    TResult Visit(Nodes.Variable node);
    TResult Visit(Nodes.DecimalLiteral node);
}
