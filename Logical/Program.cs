using Logical.Parser;
using Logical.Parser.Ast;
using Logical.Parser.Compiler;
using Newtonsoft.Json;

var parser = new Parser();
var ast = parser.Parse("x;x");
Console.WriteLine("Ast:");
Console.WriteLine(JsonConvert.SerializeObject(ast)); // Outputs "11.1".
var compiler = new Compiler(0, new Dictionary<string, List<BindingInfo<int>>>());
var compiled = compiler.Compile(ast);
Console.WriteLine("Compiled:");
Console.WriteLine(JsonConvert.SerializeObject(compiled)); // Outputs "11.1".

