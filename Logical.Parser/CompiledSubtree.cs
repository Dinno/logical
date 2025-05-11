using QuikGraph.Collections;

namespace Logical.Parser.Compiler;

public readonly struct CompiledSubtree(Model.Node node, FibonacciHeap<int, int> references)
{
    public readonly Model.Node Node = node;
    public readonly FibonacciHeap<int, int> References = references;
}
