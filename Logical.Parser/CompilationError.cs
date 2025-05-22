namespace Logical.Parser;

public readonly struct CompilationError(CompilationErrorType type)
{
    public readonly CompilationErrorType Type = type;
}
