using A.I.S.A_.Digikabu;

namespace A.I.S.A_.Core.Skills
{
    public class DigikabuSkill : AISASkill
    {
        private readonly DigikabuHandler _handler = new DigikabuHandler();

        public DigikabuSkill()
            : base()
        {
        }

        [AISAQuestion("What lessons do we have today")]
        public void GetTimeTable(TextWriter answerWriter)
        {
            if (!_handler.IsLoggedIn)
            {
                answerWriter.WriteLine("You are not logged in!");
                return;
            }

            _handler.GetLessons(answerWriter);
        }
    }
}