using Logical.Parser;
using Newtonsoft.Json;

var parser = new Parser();
var ast = parser.Parse("x;x");
Console.WriteLine("Ast:");
Console.WriteLine(JsonConvert.SerializeObject(ast)); // Outputs "11.1".
var compiler = new Compiler();
var compiled = compiler.Compile(ast);
Console.WriteLine("Compiled:");
Console.WriteLine(JsonConvert.SerializeObject(compiled)); // Outputs "11.1".

