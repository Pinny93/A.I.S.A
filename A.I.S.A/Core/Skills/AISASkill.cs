using System.Reflection;

namespace A.I.S.A_.Core.Skills;

public class AISASkill
{
    public AISASkill()
    {
        this.Questions = new Dictionary<string, IList<AISAQuestion>?>();

        // Parse all questions
        foreach (MethodInfo curMethod in this.GetType().GetMethods())
        {
            List<AISAQuestionAttribute> attr = curMethod.GetCustomAttributes<AISAQuestionAttribute>().ToList();
            if (attr.Count == 0) { continue; }

            foreach (AISAQuestionAttribute curQuestionAttribute in attr)
            {
                var question = new AISAQuestion(this, curMethod, curQuestionAttribute.Question);

                if (!this.Questions.TryGetValue(curQuestionAttribute.Language, out IList<AISAQuestion>? list) ||
                    list == null)
                {
                    this.Questions[curQuestionAttribute.Language] = list = new List<AISAQuestion>();
                }

                list.Add(question);
            }
        }
    }

    public IDictionary<string, IList<AISAQuestion>?> Questions { get; }
}