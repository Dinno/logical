using Logical.Parser.Ast;
using QuikGraph.Collections;

namespace Logical.Parser.Compiler;

public class Compiler : FullAstVisitor<int, CompiledSubtree>
{
    private List<CompilationError> Errors { get; } = [];

    public CompiledSubtree Compile(Ast.Nodes.Node ast)
    {
        return ast.Visit(this);
    }

    private static CompiledSubtree BuildBoundAbstraction(CompiledSubtree body, int level)
    {
        // Remove all variables referencing this abstraction
        foreach (var dummy in body.References.TakeWhile(pair => pair.Key == level))
        {
        }

        return new CompiledSubtree(new Model.BoundAbstraction(body.Node), body.References);
    }

    private static CompiledSubtree BuildAnnotation(CompiledSubtree annotated, CompiledSubtree annotation)
    {
        var references = CalcBinary(annotated, annotation, out var shift);
        return new CompiledSubtree(new Model.Annotation(annotated.Node, annotation.Node, shift), references);
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
        return new CompiledSubtree(new Model.Application(function.Node, argument.Node, shift), references);
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

    protected override CompiledSubtree AbstractionOrProductionOut(Ast.Nodes.AbstractionOrProduction node, Ast.BindingInfo<int> bindingInfo,
        CompiledSubtree body, CompiledSubtree? type, CompiledSubtree? annotation)
    {
        var abstraction = BuildBoundAbstraction(body, bindingInfo.Level);

        return BuildTypeAnnotation(abstraction, type, annotation);
    }

    protected override CompiledSubtree UnboundAbstractionOrProductionOut(Ast.Nodes.AbstractionOrProduction node, int level,
        CompiledSubtree body, CompiledSubtree? type, CompiledSubtree? annotation)
    {
        var abstraction = new CompiledSubtree(new Model.UnboundAbstraction(body.Node), body.References);
        return BuildTypeAnnotation(abstraction, type, annotation);
    }

    protected override CompiledSubtree ApplicationOut(Ast.Nodes.Application node, CompiledSubtree function,
        CompiledSubtree argument, CompiledSubtree? annotation)
    {
        return BuildAnnotation(BuildApplication(function, argument), annotation);
    }

    protected override CompiledSubtree PairOut(Ast.Nodes.Pair node, CompiledSubtree left, CompiledSubtree right)
    {
        throw new NotImplementedException();
    }

    protected override CompiledSubtree ParenthesesOut(Ast.Nodes.Parentheses node, CompiledSubtree @internal)
    {
        throw new NotImplementedException();
    }

    protected override CompiledSubtree Variable(Ast.Nodes.Variable node, List<Ast.BindingInfo<int>> bindings)
    {
        var references = HeapUtil.EmptyHeap;
        references.Enqueue(bindings[^1].Level, 0);
        return new CompiledSubtree(new Model.Variable(), references);
    }

    public override CompiledSubtree Visit(Ast.Nodes.DecimalLiteral node)
    {
        if (int.TryParse(node.Value, out var value))
            return Compile(BuildIntegerBinaryNumber(value));

        Errors.Add(new CompilationError(CompilationErrorType.DecimalLiteralOverflow));

        return new CompiledSubtree(new Model.Error(Errors.Count - 1), HeapUtil.EmptyHeap);
    }

    private static Ast.Nodes.Application Fun1(string name, Ast.Nodes.Node body)
    {
        return new Ast.Nodes.Application(new Ast.Nodes.Variable(name), body);
    }

    private static Ast.Nodes.Variable Var(string name)
    {
        return new Ast.Nodes.Variable(name);
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
    private static Ast.Nodes.Node BuildIntegerBinaryNumber(int value)
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
    private static Ast.Nodes.Node BuildNaturalBinaryNumber(uint value)
    {
        if (value == 0)
            throw new ArgumentException("Value must be strictly positive", nameof(value));

        var cnt = 32;
        while (cnt != 0 && (value <<= 1) == 0)
        {
            cnt--;
        }

        Ast.Nodes.Node result = Var("#xH");
        while (cnt != 0)
        {
            var bit = value & 0x80000000;
            result = Fun1(bit == 0 ? "#xO" : "#xI", result);
            value <<= 1;
            cnt--;
        }

        return result;
    }

    protected override int CreateBindingData()
    {
        return 0;
    }
}