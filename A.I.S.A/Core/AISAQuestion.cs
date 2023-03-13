using A.I.S.A_.Core.Skills;
using System.Reflection;

namespace A.I.S.A_.Core
{
    public class AISAQuestion
    {
        public AISAQuestion(AISASkill skill, MethodInfo questionMethod, string questionPrompt)
        {
            this.Skill = skill;
            this.QuestionPrompt = questionPrompt;
            this.QuestionMethod = questionMethod;
        }

        public string QuestionPrompt { get; }

        public AISASkill Skill { get; }

        public MethodInfo QuestionMethod { get; }
    }
}