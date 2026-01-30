using System;

namespace Logical.Model;

/// <summary>
/// Beta reducer for location-independent lambda calculus representation
/// </summary>
public class BetaReducer : NodeVisitorBase<Node>
{
    /// <summary>
    /// Performs one step of beta reduction on the given node
    /// </summary>
    /// <param name="node">The node to reduce</param>
    /// <returns>The reduced node, or the original node if no reduction is possible</returns>
    public static Node Reduce(Node node)
    {
        var reducer = new BetaReducer();
        return node.Accept(reducer);
    }

    /// <summary>
    /// Performs beta reduction until normal form is reached
    /// </summary>
    /// <param name="node">The node to normalize</param>
    /// <param name="maxSteps">Maximum number of reduction steps to prevent infinite loops</param>
    /// <returns>The normalized node</returns>
    public static Node Normalize(Node node, int maxSteps = 1000)
    {
        var current = node;
        for (int i = 0; i < maxSteps; i++)
        {
            var reduced = Reduce(current);
            if (ReferenceEquals(reduced, current))
                break; // No more reductions possible
            current = reduced;
        }
        return current;
    }

    public override Node Visit(Variable node) => node;

    public override Node Visit(Type node) => node;

    public override Node Visit(Error node) => node;

    public override Node Visit(BoundAbstraction node)
    {
        var reducedBody = node.Body.Accept(this);
        return ReferenceEquals(reducedBody, node.Body)
            ? node
            : new BoundAbstraction(reducedBody);
    }

    public override Node Visit(UnboundAbstraction node)
    {
        var reducedBody = node.Body.Accept(this);
        return ReferenceEquals(reducedBody, node.Body)
            ? node
            : new UnboundAbstraction(reducedBody);
    }

    public override Node Visit(Application node)
    {
        // First try to reduce the function and argument
        var reducedFunction = node.Left.Accept(this);
        var reducedArgument = node.Right.Accept(this);

        // Check if we can perform beta reduction
        if (reducedFunction is BoundAbstraction abstraction)
        {
            // Perform beta reduction: (λT)P → T[v₀ := P]
            return Substitute(abstraction.Body, reducedArgument, node.Shift);
        }

        // If no beta reduction is possible, return application with reduced components
        if (ReferenceEquals(reducedFunction, node.Left) && ReferenceEquals(reducedArgument, node.Right))
            return node;

        return new Application(reducedFunction, reducedArgument, node.Shift);
    }

    public override Node Visit(Production node)
    {
        var reducedLeft = node.Left.Accept(this);
        var reducedRight = node.Right.Accept(this);

        if (ReferenceEquals(reducedLeft, node.Left) && ReferenceEquals(reducedRight, node.Right))
            return node;

        return new Production(reducedLeft, reducedRight, node.Shift);
    }

    public override Node Visit(Annotation node)
    {
        var reducedLeft = node.Left.Accept(this);
        var reducedRight = node.Right.Accept(this);

        if (ReferenceEquals(reducedLeft, node.Left) && ReferenceEquals(reducedRight, node.Right))
            return node;

        return new Annotation(reducedLeft, reducedRight, node.Shift);
    }

    /// <summary>
    /// Substitutes variables with index 0 in the target term with the replacement term
    /// </summary>
    /// <param name="target">The term in which to perform substitution</param>
    /// <param name="replacement">The term to substitute</param>
    /// <param name="shift">The shift value from the application</param>
    /// <returns>The term with substitution performed</returns>
    private static Node Substitute(Node target, Node replacement, int shift)
    {
        var substituter = new Substituter(replacement, shift);
        return target.Accept(substituter);
    }

    /// <summary>
    /// Internal visitor for performing variable substitution
    /// </summary>
    private class Substituter : NodeVisitorBase<Node>
    {
        private readonly Node _replacement;
        private readonly int _shift;
        private int _depth;

        public Substituter(Node replacement, int shift)
        {
            _replacement = replacement;
            _shift = shift;
            _depth = 0;
        }

        public override Node Visit(Variable node)
        {
            // In location-independent representation, we substitute variables with index 0
            // The actual index is determined by the context and shift values
            if (_depth == 0)
            {
                // This is the variable we want to substitute
                return ShiftIndices(_replacement, _shift);
            }
            return node;
        }

        public override Node Visit(Type node) => node;

        public override Node Visit(Error node) => node;

        public override Node Visit(BoundAbstraction node)
        {
            // Entering a bound abstraction increases the depth
            _depth++;
            var substitutedBody = node.Body.Accept(this);
            _depth--;

            return ReferenceEquals(substitutedBody, node.Body)
                ? node
                : new BoundAbstraction(substitutedBody);
        }

        public override Node Visit(UnboundAbstraction node)
        {
            // Unbound abstractions don't bind variables, so depth doesn't change
            var substitutedBody = node.Body.Accept(this);
            return ReferenceEquals(substitutedBody, node.Body)
                ? node
                : new UnboundAbstraction(substitutedBody);
        }

        public override Node Visit(Application node)
        {
            var substitutedLeft = node.Left.Accept(this);
            var substitutedRight = node.Right.Accept(this);

            if (ReferenceEquals(substitutedLeft, node.Left) && ReferenceEquals(substitutedRight, node.Right))
                return node;

            return new Application(substitutedLeft, substitutedRight, node.Shift);
        }

        public override Node Visit(Production node)
        {
            var substitutedLeft = node.Left.Accept(this);
            var substitutedRight = node.Right.Accept(this);

            if (ReferenceEquals(substitutedLeft, node.Left) && ReferenceEquals(substitutedRight, node.Right))
                return node;

            return new Production(substitutedLeft, substitutedRight, node.Shift);
        }

        public override Node Visit(Annotation node)
        {
            var substitutedLeft = node.Left.Accept(this);
            var substitutedRight = node.Right.Accept(this);

            if (ReferenceEquals(substitutedLeft, node.Left) && ReferenceEquals(substitutedRight, node.Right))
                return node;

            return new Annotation(substitutedLeft, substitutedRight, node.Shift);
        }
    }
}