using System;

namespace Logical.Ast.Nodes;

public class DecimalLiteral(string value, Node? annotation = null) : Node(annotation)
{
    public readonly string Value = value;
    public readonly Node? IntegerBinary = BuildIntegerBinaryNumber(value);
    
    public override TResult Visit<TResult>(IAstVisitor<TResult> astVisitor)
    {
        return astVisitor.Visit(this);
    }
    
    private static Application Fun1(string name, Node body)
    {
        return new Application(new Variable(name), body);
    }

    private static Variable Var(string name)
    {
        return new Variable(name);
    }

    private static Node? BuildIntegerBinaryNumber(string literal)
    {
        return int.TryParse(literal, out var value) ? BuildIntegerBinaryNumber(value) : null;
    }

    /// <summary>
    /// Build an AST representation of integer binary number.
    /// </summary>
    /// <param name="value">Integer number</param>
    /// <returns>AST representing the number</returns>
    /// <remarks>
    /// Representation of such numbers is taken from the Coq library.
    /// https://coq.inria.fr/library/Coq.Numbers.BinNums.html
    /// </remarks>
    private static Node BuildIntegerBinaryNumber(int value)
    {
        if (value == 0)
            return Var("#binIntZero");
        return value > 0
            ? Fun1("#binIntPos", BuildPositiveBinaryNumber((uint)value))
            : Fun1("#binIntNeg", BuildPositiveBinaryNumber((uint)-value));
    }

    /// <summary>
    /// Build an AST representation of a strictly positive binary number.
    /// </summary>
    /// <param name="value">Strictly positive number</param>
    /// <returns>AST representing the number</returns>
    /// <remarks>
    /// Representation of such numbers is taken from the Coq library.
    /// https://coq.inria.fr/library/Coq.Numbers.BinNums.html
    /// </remarks>
    private static Node BuildPositiveBinaryNumber(uint value)
    {
        if (value == 0)
            throw new ArgumentException("Value must be strictly positive", nameof(value));

        var cnt = 31;
        while ((value & 0x80000000) == 0)
        {
            cnt--;
            value <<= 1;
        }
        value <<= 1;

        Node result = Var("#binPosHead");
        while (cnt != 0)
        {
            var bit = value & 0x80000000;
            result = Fun1(bit == 0 ? "#binPosZero" : "#binPosOne", result);
            value <<= 1;
            cnt--;
        }

        return result;
    }
}
