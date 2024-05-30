﻿namespace LogicalParser.Model;

public class Annotation : BinaryNode
{
    public Annotation(Node annotated, Node annotation, int shift) : base(annotated, annotation, shift)
    {
    }
}