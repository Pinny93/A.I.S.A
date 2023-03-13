namespace A.I.S.A_.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class AISAQuestionAttribute : Attribute
    {
        public AISAQuestionAttribute(string question, string language = "en-US")
        {
            this.Question = question;
            this.Language = language;
        }

        public string Language { get; set; }

        public string Question { get; set; }
    }
}