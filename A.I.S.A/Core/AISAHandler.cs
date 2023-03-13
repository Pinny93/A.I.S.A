using A.I.S.A_.Core.Skills;

namespace A.I.S.A_.Core
{
    public class AISAHandler
    {
        public AISAHandler()
        {
            // TODO: Get all loaded types derived from AISASkill
            this.Skills = new List<AISASkill>()
            {
                new DigikabuSkill(),
            };
        }

        public void Say(string message, TextWriter answerWriter)
        {
            char[] trimChars = new char[] { ' ', '?', '.', '!' };

            string prompt = message.ToLower().Trim(trimChars);
                AISAQuestion? foundQuestion = null;

            foreach (AISASkill curSkill in this.Skills)
            {
                const string LANGUAGE = "en-US";
                IList<AISAQuestion>? questions = curSkill.Questions[LANGUAGE];
                if (questions == null) { continue; }

                foreach (AISAQuestion curQuestion in questions)
                {
                    if (curQuestion.QuestionPrompt.ToLower().Trim(trimChars) == prompt)
                    {
                        foundQuestion = curQuestion;
                        break;
                    }
                }

                if (foundQuestion != null)
                {
                    break;
                }
            }

            if (foundQuestion != null)
            {
                foundQuestion.QuestionMethod.Invoke(foundQuestion.Skill, new object[] { answerWriter });
            }
            else
            {
                // Fixed fallback cases
                switch (prompt)
                {
                    case "hello":
                        answerWriter.WriteLine("Hello, I'm A.I.S.A.!");
                        break;

                    default:
                        answerWriter.WriteLine("Don't know what you mean. Ask GPT!");
                        break;
                }
            }
            
        }

        public IList<AISASkill> Skills { get; }
    }
}