namespace Logical.Parser.Compiler;

public readonly struct CompilationError
{
    public readonly CompilationErrorType Type;

    public CompilationError(CompilationErrorType type)
    {
        Type = type;
    }
}
