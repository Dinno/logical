using LogicalParser.Ast;
using LogicalParser.Ast.Nodes;
using LogicalParser.Model;
using QuikGraph.Collections;
using Application = LogicalParser.Model.Application;
using Node = LogicalParser.Ast.Nodes.Node;
using Variable = LogicalParser.Ast.Nodes.Variable;

namespace LogicalParser.Compiler;

public class Compiler : IAstVisitor<int, CompiledSubtree>
{
    public List<CompilationError> Errors { get; } = new();

    public CompiledSubtree Compile(Node ast)
    {
        var traversal = new AstTraversal<int, CompiledSubtree, Compiler>(this);
        return traversal.Traverse(ast);
    }

    private static CompiledSubtree BuildBoundAbstraction(CompiledSubtree body, int level)
    {
        // Remove all variables referencing this abstraction
        foreach (var dummy in body.References.TakeWhile(pair => pair.Key == level))
        {
        }

        return new CompiledSubtree(new BoundAbstraction(body.Node), body.References);
    }

    private static CompiledSubtree BuildAnnotation(CompiledSubtree annotated, CompiledSubtree annotation)
    {
        var references = CalcBinary(annotated, annotation, out var shift);
        return new CompiledSubtree(new Annotation(annotated.Node, annotation.Node, shift), references);
    }

    private static CompiledSubtree BuildAnnotation(CompiledSubtree annotated, CompiledSubtree? annotation)
    {
        return annotation.HasValue
            ? BuildAnnotation(annotated, annotation.Value)
            : annotated;
    }

    private static CompiledSubtree BuildApplication(CompiledSubtree function, CompiledSubtree argument)
    {
        var references = CalcBinary(function, argument, out var shift);
        return new CompiledSubtree(new Application(function.Node, argument.Node, shift), references);
    }

    private static FibonacciHeap<int, int> CalcBinary(CompiledSubtree annotated, CompiledSubtree annotation,
        out int shift)
    {
        var leftLevel = -annotated.References.Top.Priority;
        var rightLevel = -annotation.References.Top.Priority;
        annotated.References.Merge(annotation.References);
        shift = rightLevel - leftLevel;
        return annotated.References;
    }

    private static CompiledSubtree BuildTypeAnnotation(CompiledSubtree abstraction, CompiledSubtree? type,
        CompiledSubtree? _)
    {
        // TODO: Handle non-type annotations
        return type.HasValue
            ? BuildAnnotation(abstraction, type.Value)
            : abstraction;
    }

    public CompiledSubtree AbstractionOrProductionOut(AbstractionOrProduction node, BindingInfo<int> bindingInfo,
        CompiledSubtree body, CompiledSubtree? type, CompiledSubtree? annotation)
    {
        var abstraction = BuildBoundAbstraction(body, bindingInfo.Level);

        return BuildTypeAnnotation(abstraction, type, annotation);
    }

    public CompiledSubtree UnboundAbstractionOrProductionOut(AbstractionOrProduction node, int level,
        CompiledSubtree body, CompiledSubtree? type, CompiledSubtree? annotation)
    {
        var abstraction = new CompiledSubtree(new UnboundAbstraction(body.Node), body.References);
        return BuildTypeAnnotation(abstraction, type, annotation);
    }

    public CompiledSubtree ApplicationOut(Ast.Nodes.Application node, CompiledSubtree function,
        CompiledSubtree argument, CompiledSubtree? annotation)
    {
        return BuildAnnotation(BuildApplication(function, argument), annotation);
    }

    public CompiledSubtree PairOut(Pair node, CompiledSubtree left, CompiledSubtree right)
    {
        throw new NotImplementedException();
    }

    public CompiledSubtree ParenthesesOut(Parentheses node, CompiledSubtree @internal)
    {
        throw new NotImplementedException();
    }

    public CompiledSubtree Variable(Variable node, List<BindingInfo<int>> bindings)
    {
        var references = HeapUtil.EmptyHeap;
        references.Enqueue(bindings[^1].Level, 0);
        return new CompiledSubtree(new Model.Variable(), references);
    }

    public CompiledSubtree DecimalLiteral(DecimalLiteral node)
    {
        if (int.TryParse(node.Value, out var value))
            return Compile(BuildIntegerBinaryNumber(value));

        Errors.Add(new CompilationError(CompilationErrorType.DecimalLiteralOverflow));

        return new CompiledSubtree(new Error(Errors.Count - 1), HeapUtil.EmptyHeap);
    }

    private static Node Fun1(string name, Node body)
    {
        return new Ast.Nodes.Application(new Variable(name), body);
    }

    private static Node Var(string name)
    {
        return new Variable(name);
    }

    /// <summary>
    /// Build an AST representation of integer binary number.
    /// </summary>
    /// <param name="value">Integer number</param>
    /// <returns>AST representing the number</returns>
    /// <remarks>
    /// Representation of such numbers is taken from the Coq library.
    /// https://coq.inria.fr/library/Coq.Numbers.BinNums.html
    /// </remarks>
    private static Node BuildIntegerBinaryNumber(int value)
    {
        if (value == 0)
            return Var("#Z0");
        return value > 0
            ? Fun1("#Zpos", BuildNaturalBinaryNumber((uint)value))
            : Fun1("#Zneg", BuildNaturalBinaryNumber((uint)-value));
    }

    /// <summary>
    /// Build an AST representation of a strictly positive binary number.
    /// </summary>
    /// <param name="value">Strictly positive number</param>
    /// <returns>AST representing the number</returns>
    /// <remarks>
    /// Representation of such numbers is taken from the Coq library.
    /// https://coq.inria.fr/library/Coq.Numbers.BinNums.html
    /// </remarks>
    private static Node BuildNaturalBinaryNumber(uint value)
    {
        if (value == 0)
            throw new ArgumentException("Value must be strictly positive", nameof(value));

        var cnt = 32;
        while (cnt != 0 && (value <<= 1) == 0)
        {
            cnt--;
        }

        var result = Var("#xH");
        while (cnt != 0)
        {
            var bit = value & 0x80000000;
            result = Fun1(bit == 0 ? "#xO" : "#xI", result);
            value <<= 1;
            cnt--;
        }

        return result;
    }

    public int CreateBindingData()
    {
        return 0;
    }
}