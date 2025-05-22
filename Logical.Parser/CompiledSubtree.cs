using Logical.Model;
using QuikGraph.Collections;

namespace Logical.Parser;

public readonly struct CompiledSubtree(Node node, FibonacciHeap<int, int> references)
{
    public readonly Node Node = node;
    public readonly FibonacciHeap<int, int> References = references;
}
