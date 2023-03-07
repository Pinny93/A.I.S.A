using A.I.S.A.PythonEngine;
using A.I.S.A.Utils;
using A.I.S.A_.DBModel;
using A.I.S.A_.GPT;
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
        public const string API_KEY_FILE = @"OpenAI.API.key";
        private PythonEngine? _engine;

        public AISA()
        {
            this.DBContext = new AisaDBContext();
        }

        // f(x) = x + 4*x**2 - 3*x**3 + 7*x**3 << WTF?
        public void Help()
        {
            Console.WriteLine("This system is not implemented yes. No commands available!");
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void GPT(string prompt, int token = 150)
        {
            string key = File.ReadAllText(API_KEY_FILE);

            var client = new RestClient("https://api.openai.com/v1");
            var request = new RestRequest("completions", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {key}");
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
                Console.Error.WriteLine("Error occurred: " + response.StatusCode);
                return;
            }

            if (response.Content == null)
            {
                Console.Error.WriteLine("Empty response!");
                return;
            }

            var responseObj = JsonSerializer.Deserialize<ChatGptResponse>(response.Content);
            string answer = responseObj.Choices.FirstOrDefault()?.Text ?? "Keine Auswahl.";

            Console.WriteLine(answer);

            //foreach (ChatGptChoice curChoice in responseObj.Choices)
            //{
            //    Console.WriteLine(curChoice.Text);
            //    Console.WriteLine("---");
            //}

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
            string key = File.ReadAllText(API_KEY_FILE);

            _engine ??= new PythonEngine();
            _engine.GPT(key, prompt);
        }

        public void PrintQueries()
        {
            foreach (Queries curQuery in this.DBContext.Queries)
            {
                Console.WriteLine($"{curQuery.Id}: {curQuery.Prompt.Truncate(100).EscapeNewLine(),-100} :: {curQuery.Answer?.Truncate(100).EscapeNewLine() ?? curQuery.AnswerFull.Truncate(100).EscapeNewLine()}");
            }
        }

        public AisaDBContext DBContext { get; set; }
    }
}