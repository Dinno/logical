namespace Logical.Parser.Compiler;

public readonly struct CompilationError(CompilationErrorType type)
{
    public readonly CompilationErrorType Type = type;
}
