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
        public const string API_KEY_FILE = "OpenAI.API.key";

        public AISA()
        {
            this.DBContext = new AisaDBContext();
        }

        // f(x) = x + 4*x**2 - 3*x**3 + 7*x**3 << WTF?
        public void Help()
        {
            Console.WriteLine("This system is not implemented yes. No commands available!");
        }

        public void GPT(string prompt)
        {
            string key = File.ReadAllText(API_KEY_FILE);

            var client = new RestClient("https://api.openai.com/v1/");
            var request = new RestRequest("engines/davinci-codex/completions", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {key}");

            request.AddParameter("application/json", $"{{\n  \"prompt\": \"{prompt}\",\n  \"max_tokens\": 500,\n  \"temperature\": 0.7\n}}", ParameterType.RequestBody);

            RestResponse response = client.Execute(request);
            if (!response.IsSuccessful)
            {
                Console.Error.WriteLine("Error occurred: " + response.StatusCode);
                return;
            }

            var responseObj = JsonSerializer.Deserialize<ChatGptResponse>(response.Content ?? String.Empty);

            Console.WriteLine(response.Content);

            //foreach (ChatGptChoice curChoice in responseObj.Choices)
            //{
            //    Console.WriteLine(curChoice.Text);
            //    Console.WriteLine("---");
            //}

            Queries q = new()
            {
                Id = Guid.NewGuid(),
                Prompt = prompt,
                Answer = "",
                AnswerFull = response.Content ?? String.Empty,
            };

            this.DBContext.Queries.Add(q);
            this.DBContext.SaveChanges();
        }

        public void PrintQueries()
        {
            foreach (Queries curQuery in this.DBContext.Queries)
            {
                Console.WriteLine($"{curQuery.Id}: {curQuery.Prompt,-100} :: {curQuery.Answer ?? curQuery.AnswerFull}");
            }
        }

        public AisaDBContext DBContext { get; set; }
    }
}