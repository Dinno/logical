namespace Logical.Parser.Model;

public class Production(Node left, Node right, int shift) : BinaryNode(left, right, shift);
