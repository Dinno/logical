namespace Logical.Parser.Model;

public class Annotation(Node annotated, Node annotation, int shift) : BinaryNode(annotated, annotation, shift);
