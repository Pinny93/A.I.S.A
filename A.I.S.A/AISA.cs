using A.I.S.A.PythonEngine;
using A.I.S.A.Utils;
using A.I.S.A_.Core;
using A.I.S.A_.DBModel;
using A.I.S.A_.Digikabu;
using A.I.S.A_.GPT;
using CommandDotNet;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace A.I.S.A_
{
    public class AISA
    {
        public const string API_KEY_FILE = @"Keys\OpenAI.API.key";

        private readonly AISAHandler _aisaHandler = new AISAHandler();
        private PythonEngine? _engine;

        public AISA()
        {
            if (File.Exists(API_KEY_FILE)) { this.ApiKey = File.ReadAllText(API_KEY_FILE); }

            this.DBContext = new AisaDBContext();
        }

        public void Help()
        {
            AnswerWriter.WriteLine("This system is not implemented yes. No commands available!");
        }

        public void Clear()
        {
            if (AnswerWriter == Console.Out)
            {
                Console.Clear();
            }
            else
            {
                AnswerWriter.WriteLine("Not supported on this terminal host.");
            }
        }

        [DefaultCommand]
        public void Say(string message)
        {
            _aisaHandler.Say(message, AnswerWriter);
        }

        public void GPT(string prompt, int token = 150)
        {
            var client = new RestClient("https://api.openai.com/v1");
            var request = new RestRequest("completions", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {this.ApiKey}");
            request.AddJsonBody(
                $$"""
                    {
                        "model": "text-davinci-003",
                        "prompt": "{{prompt}}",
                        "temperature": 0.8,
                        "max_tokens": 150
                    }
                    """
                );

            RestResponse response = client.Execute(request);
            if (!response.IsSuccessful)
            {
                ErrorWriter.WriteLine("Error occurred: " + response.StatusCode);
                return;
            }

            if (response.Content == null)
            {
                ErrorWriter.WriteLine("Empty response!");
                return;
            }

            var responseObj = JsonSerializer.Deserialize<ChatGptResponse>(response.Content);
            string answer = responseObj.Choices.FirstOrDefault()?.Text ?? "Keine Auswahl.";

            AnswerWriter.WriteLine(answer);

            Queries q = new()
            {
                Id = Guid.NewGuid(),
                Prompt = prompt,
                Answer = answer,
                AnswerFull = response.Content ?? String.Empty,
            };

            this.DBContext.Queries.Add(q);
            this.DBContext.SaveChanges();
        }

        public void Py(string code)
        {
            _engine ??= new PythonEngine();
            _engine.Execute(code);
        }

        public void PyGPT(string prompt)
        {
            _engine ??= new PythonEngine();
            _engine.GPT(this.ApiKey, prompt);
        }

        public void PrintQueries()
        {
            foreach (Queries curQuery in this.DBContext.Queries)
            {
                AnswerWriter.WriteLine($"{curQuery.Id}: {curQuery.Prompt.Truncate(100).EscapeNewLine(),-100} :: {curQuery.Answer?.Truncate(100).EscapeNewLine() ?? curQuery.AnswerFull.Truncate(100).EscapeNewLine()}");
            }
        }

        public static TextWriter AnswerWriter { get; set; } = Console.Out;

        public static TextWriter ErrorWriter { get; set; } = Console.Error;

        public static bool IsInteractive
        {
            get;
            internal set;
        }

        [Subcommand(RenameAs = "Digikabu")]
        public DigikabuCmds DigikabuCommands { get; } = new DigikabuCmds();

        public string ApiKey { get; set; }
        public AisaDBContext DBContext { get; set; }
    }
}