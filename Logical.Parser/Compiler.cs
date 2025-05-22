using System;
using System.Collections.Generic;
using System.Linq;
using Logical.Ast;
using Logical.Model;
using QuikGraph.Collections;

namespace Logical.Parser;

public class Compiler : FullAstVisitor<int, CompiledSubtree>
{
    private List<CompilationError> Errors { get; } = [];

    /// <summary>
    /// Создаёт новый экземпляр компилятора.
    /// </summary>
    public Compiler() : base()
    {
    }

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

    private static FibonacciHeap<int, int> CalcBinary(CompiledSubtree left, CompiledSubtree right, out int shift)
    {
        shift = right.References.Top.Priority - left.References.Top.Priority;
        left.References.Merge(right.References);
        return left.References;
    }

    private static CompiledSubtree BuildTypeAnnotation(CompiledSubtree abstraction, CompiledSubtree? type,
        CompiledSubtree? _)
    {
        // TODO: Handle non-type annotations
        return type.HasValue
            ? BuildAnnotation(abstraction, type.Value)
            : abstraction;
    }

    protected override CompiledSubtree AbstractionOrProductionOut(Ast.Nodes.AbstractionOrProduction node,
        Ast.BindingInfo<int> bindingInfo,
        CompiledSubtree body, CompiledSubtree? type, CompiledSubtree? annotation)
    {
        var abstraction = BuildBoundAbstraction(body, bindingInfo.Level);

        return BuildTypeAnnotation(abstraction, type, annotation);
    }

    protected override CompiledSubtree UnboundAbstractionOrProductionOut(Ast.Nodes.AbstractionOrProduction node,
        int level,
        CompiledSubtree body, CompiledSubtree? type, CompiledSubtree? annotation)
    {
        var abstraction = new CompiledSubtree(new UnboundAbstraction(body.Node), body.References);
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

    /// <summary>
    /// Вызывается при выходе из переменной.
    /// </summary>
    /// <param name="node">Узел переменной АСД</param>
    /// <param name="bindings">Список связей этой переменной. Переменная может быть связана с несколькими абстракциями</param>
    /// <returns>Результат компиляции</returns>
    protected override CompiledSubtree Variable(Ast.Nodes.Variable node, List<Ast.BindingInfo<int>> bindings)
    {
        var references = HeapUtil.EmptyHeap;

        // Пока что мы всегда считаем, что переменная связана с последней абстракцией в списке
        references.Enqueue(-bindings[^1].Level, 0 /* Is not used */);

        return new CompiledSubtree(new Variable(), references);
    }

    protected override CompiledSubtree OnVariableNotFoundError(Ast.Nodes.Variable node)
    {
        throw new NotImplementedException();
    }

    protected override int CreateBindingData()
    {
        return 0;
    }

    protected override CompiledSubtree OnDecimalLiteralError()
    {
        Errors.Add(new CompilationError(CompilationErrorType.DecimalLiteralOverflow));

        return new CompiledSubtree(new Error(Errors.Count - 1), HeapUtil.EmptyHeap);
    }
}