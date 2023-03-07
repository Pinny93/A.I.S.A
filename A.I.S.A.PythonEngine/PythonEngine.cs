using A.I.S.A.Utils;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using System.Text;

namespace A.I.S.A.PythonEngine
{
    public class PythonEngine
    {
        private const string OPENAI_MODULE_PATH = @".\pythonlibs\openai-python";
        private const string CONTEXTVARS_MODULE_PATH = @".\pythonlibs\contextvars";
        private const string IMMUTABLES_MODULE_PATH = @".\pythonlibs\immutables";
        private const string TYPING_MODULE_PATH = @".\pythonlibs\typing";
        private const string TYPING_EXT_MODULE_PATH = @".\pythonlibs\typing_extensions";
        private const string TYPING_EXT_TYPES_MODULE_PATH = @".\pythonlibs\types_typing_extensions";

        private static readonly string CUR_PYTHON_PATH = @"C:\Python34";
        private readonly ScriptEngine _engine;
        private readonly MemoryStream _stream;

        private readonly string[] ENGINE_SEARCH_PATHS = new[]
        {
            OPENAI_MODULE_PATH,
            CONTEXTVARS_MODULE_PATH,
            IMMUTABLES_MODULE_PATH,
            TYPING_MODULE_PATH,
            TYPING_EXT_TYPES_MODULE_PATH,
            TYPING_EXT_MODULE_PATH,
        };

        public PythonEngine()
        {
            // Erstelle ein neues ScriptRuntime
            ScriptRuntimeSetup setup = Python.CreateRuntimeSetup(null);
            ScriptRuntime runtime = new ScriptRuntime(setup);

            // Erstelle ein neues ScriptEngine
            _engine = runtime.GetEngineByTypeName(typeof(PythonContext).AssemblyQualifiedName);

            // Add Module Paths
            ICollection<string> paths = _engine.GetSearchPaths();
            paths.Add(Environment.CurrentDirectory);
            paths.Add(Path.Join(CUR_PYTHON_PATH, "Lib"));

            foreach (string curPath in ENGINE_SEARCH_PATHS)
            {
                //string dir = Path.GetDirectoryName(curPath) ?? throw new InvalidOperationException("Path must not be empty!");
                paths.Add(Path.GetFullPath(curPath));
            }

            _engine.SetSearchPaths(paths);

            _stream = new MemoryStream();
            _engine.Runtime.IO.SetOutput(_stream, Encoding.ASCII);
        }

        public void Execute(string code)
        {
            // Führe Python-Code aus
            _engine.Execute(code);
        }

        public string GPT(string apiKey, string prompt, int maxToken = 150, double temperature = 0.9)
        {
            Console.WriteLine(_engine.LanguageVersion);

            string pythonCode = $"""
                    def execute_chat_gpt():
                        import openai
                        openai.api_key = "{apiKey}"
                        response = openai.Completion.create(
                            engine="davinci",
                            prompt="{prompt}",
                            max_tokens={maxToken}
                        )
                        print(response)

                    execute_chat_gpt()
                    """;
            _engine.Execute(pythonCode);

            string output = new StreamReader(_stream).ReadToEnd();
            _stream.Clear();

            return output;
        }
    }
}