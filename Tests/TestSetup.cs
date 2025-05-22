using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace Tests
{
    public static class TestSetup
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            };
        }
    }
}