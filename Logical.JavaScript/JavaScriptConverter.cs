using Logical.Parser.Model;
using System.Text;

namespace Logical.JavaScript;

public class JavaScriptConverter(Stream stream)
{
    private Abstraction? _abstractionChain = null;

    public void ConvertToJavaScript(Node node)
    {
        if (node is Annotation annotation)
        {
            MatchCommonNode(annotation.Left, annotation.Right);
        }
        else
        {
            MatchCommonNode(node, null);
        }
    }

    private void MatchCommonNode(Node node, Node? annotation)
    {
        switch (node)
        {
            case Abstraction abstraction:
                ConvertToJavaScript(abstraction, annotation);
                break;
            case Application application:
                ConvertToJavaScript(application, annotation);
                break;
            case Variable variable:
                ConvertToJavaScript(variable, annotation);
                break;
            default:
                throw new ArgumentException($"Unknown node type: {node.GetType().Name}");
        }
    }

    private void ConvertToJavaScript(Abstraction abstraction, Node? annotation)
    {
        if (annotation is not null && FindAnnotationAttribute(annotation, "IsJavaScriptInternal") != null)
        {
            // Не нужно определять функцию, если она помечена как JavaScript функция, 
            // потому что она уже определена в JavaScript коде
            return;
        }
        _abstractionChain ??= abstraction;

        var builder = new StringBuilder();

        builder.Append($"function {abstraction.Name}(");
        builder.Append(string.Join(", ", abstraction.Parameters));
        builder.Append(") {\n");

        foreach (var node in abstraction.Body)
        {
            ConvertToJavaScript(node);
        }

        builder.Append("}\n\n");

        WriteToStream(builder.ToString());
    }

    private void ConvertToJavaScript(Application application, Node? annotation)
    {
        throw new NotImplementedException();
    }

    private void ConvertToJavaScript(Variable variable, Node? annotation)
    {
        throw new NotImplementedException();
    }

    private void WriteToStream(string code)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(code);
        stream.Write(bytes, 0, bytes.Length);
    }

    private Node? FindAnnotationAttribute(Node attributeList, string attributeName)
    {
        // TODO: Implement
        return attributeList;
    }
}