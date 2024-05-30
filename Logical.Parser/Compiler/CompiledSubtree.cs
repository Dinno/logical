using LogicalParser.Model;
using QuikGraph.Collections;

namespace LogicalParser.Compiler;

public readonly struct CompiledSubtree
{
    public readonly Node Node;
    public readonly FibonacciHeap<int, int> References;

    public CompiledSubtree(Node node, FibonacciHeap<int, int> references)
    {
        Node = node;
        References = references;
    }
}
