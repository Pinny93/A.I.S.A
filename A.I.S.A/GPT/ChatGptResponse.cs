using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace A.I.S.A_.GPT
{
    public class ChatGptResponse
    {
        [JsonPropertyName("choices")]
        public List<ChatGPTChoice>? Choices
        {
            get;
            set;
        }

        [JsonPropertyName("usage")]
        public ChatGPTUsage? Usage
        {
            get;
            set;
        }
    }

    public class ChatGPTUsage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens
        {
            get;
            set;
        }

        [JsonPropertyName("completion_token")]
        public int CompletionTokens
        {
            get;
            set;
        }

        [JsonPropertyName("total_tokens")]
        public int TotalTokens
        {
            get;
            set;
        }
    }

    [DebuggerDisplay("Text = {Text}")]
    public class ChatGPTChoice
    {
        [JsonPropertyName("text")]
        public string? Text
        {
            get;
            set;
        }
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