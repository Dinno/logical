using System;

namespace Logical.Model;

/// <summary>
/// Visitor interface for traversing Node trees without return values
/// </summary>
public interface INodeVisitor
{
    void Visit(Variable node);
    void Visit(Type node);
    void Visit(Error node);
    void Visit(BoundAbstraction node);
    void Visit(UnboundAbstraction node);
    void Visit(Application node);
    void Visit(Production node);
    void Visit(Annotation node);
}

/// <summary>
/// Generic visitor interface for traversing Node trees with return values
/// </summary>
/// <typeparam name="T">The return type of visit methods</typeparam>
public interface INodeVisitor<out T>
{
    T Visit(Variable node);
    T Visit(Type node);
    T Visit(Error node);
    T Visit(BoundAbstraction node);
    T Visit(UnboundAbstraction node);
    T Visit(Application node);
    T Visit(Production node);
    T Visit(Annotation node);
}

/// <summary>
/// Extension methods to enable visitor pattern on Node instances
/// </summary>
public static class NodeVisitorExtensions
{
    public static void Accept(this Node node, INodeVisitor visitor)
    {
        switch (node)
        {
            case Variable variable:
                visitor.Visit(variable);
                break;
            case Type type:
                visitor.Visit(type);
                break;
            case Error error:
                visitor.Visit(error);
                break;
            case BoundAbstraction boundAbstraction:
                visitor.Visit(boundAbstraction);
                break;
            case UnboundAbstraction unboundAbstraction:
                visitor.Visit(unboundAbstraction);
                break;
            case Application application:
                visitor.Visit(application);
                break;
            case Production production:
                visitor.Visit(production);
                break;
            case Annotation annotation:
                visitor.Visit(annotation);
                break;
            default:
                throw new ArgumentException($"Unknown node type: {node.GetType()}");
        }
    }

    public static T Accept<T>(this Node node, INodeVisitor<T> visitor)
    {
        return node switch
        {
            Variable variable => visitor.Visit(variable),
            Type type => visitor.Visit(type),
            Error error => visitor.Visit(error),
            BoundAbstraction boundAbstraction => visitor.Visit(boundAbstraction),
            UnboundAbstraction unboundAbstraction => visitor.Visit(unboundAbstraction),
            Application application => visitor.Visit(application),
            Production production => visitor.Visit(production),
            Annotation annotation => visitor.Visit(annotation),
            _ => throw new ArgumentException($"Unknown node type: {node.GetType()}")
        };
    }
}

/// <summary>
/// Base visitor class that provides default traversal behavior
/// </summary>
public abstract class NodeVisitorBase : INodeVisitor
{
    public virtual void Visit(Variable node) { }

    public virtual void Visit(Type node) { }

    public virtual void Visit(Error node) { }

    public virtual void Visit(BoundAbstraction node)
    {
        node.Body.Accept(this);
    }

    public virtual void Visit(UnboundAbstraction node)
    {
        node.Body.Accept(this);
    }

    public virtual void Visit(Application node)
    {
        node.Left.Accept(this);
        node.Right.Accept(this);
    }

    public virtual void Visit(Production node)
    {
        node.Left.Accept(this);
        node.Right.Accept(this);
    }

    public virtual void Visit(Annotation node)
    {
        node.Left.Accept(this);
        node.Right.Accept(this);
    }
}

/// <summary>
/// Base generic visitor class that provides default traversal behavior with return values
/// </summary>
/// <typeparam name="T">The return type of visit methods</typeparam>
public abstract class NodeVisitorBase<T> : INodeVisitor<T>
{
    public abstract T Visit(Variable node);
    public abstract T Visit(Type node);
    public abstract T Visit(Error node);

    public virtual T Visit(BoundAbstraction node)
    {
        return node.Body.Accept(this);
    }

    public virtual T Visit(UnboundAbstraction node)
    {
        return node.Body.Accept(this);
    }

    public virtual T Visit(Application node)
    {
        node.Left.Accept(this);
        return node.Right.Accept(this);
    }

    public virtual T Visit(Production node)
    {
        node.Left.Accept(this);
        return node.Right.Accept(this);
    }

    public virtual T Visit(Annotation node)
    {
        node.Left.Accept(this);
        return node.Right.Accept(this);
    }
}
