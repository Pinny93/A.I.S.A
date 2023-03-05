using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A.I.S.A_.GPT
{
    public class ChatGptResponse
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public long Created { get; set; }
        public string Model { get; set; }
        public List<ChatGptChoice> Choices { get; set; }
        public ChatGptUsage Usage { get; set; }
    }

    public class ChatGptChoice
    {
        public string Text { get; set; }
        public int Index { get; set; }
        public double Logprobs { get; set; }
        public double LogprobsToken { get; set; }
        public List<double> LogprobsTop { get; set; }
    }

    public class ChatGptUsage
    {
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int TotalTokens { get; set; }
    }
}