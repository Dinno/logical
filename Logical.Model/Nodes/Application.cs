namespace Logical.Parser.Model;

public class Application(Node function, Node argument, int shift) : BinaryNode(function, argument, shift);
