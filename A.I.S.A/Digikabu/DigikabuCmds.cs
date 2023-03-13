namespace A.I.S.A_.Digikabu
{
    public class DigikabuCmds
    {
        private readonly DigikabuHandler _digikabuHandler;

        public DigikabuCmds()
        {
            _digikabuHandler = new DigikabuHandler();
        }

        public void Login(string user, string? password = null)
        {
            _digikabuHandler.Login(user, AISA.AnswerWriter, password);
        }

        public void Logout()
        {
            if (!_digikabuHandler.EnsureLoggedIn(AISA.AnswerWriter)) { return; }

            _digikabuHandler.Logout();
        }

        public void PrintTimeTable(int group = -1, DateTime date = default, int days = 1)
        {
            DateTime curDay = date;
            foreach (DigikabuTimeTableLesson curLesson in _digikabuHandler.GetLessons(AISA.AnswerWriter, date, days))
            {
                if (curLesson.Datum != date)
                {
                    Console.WriteLine($"{curLesson.Datum:dd.MM.yyyy}");
                    Console.WriteLine("--------------------------");
                    date = curLesson.Datum;
                }

                string groupStr = $" ({curLesson.Gruppe})";
                if (group > 0)
                {
                    string[] groupFields = curLesson.Gruppe.Split('/');
                    int curGroup = Int32.Parse(groupFields[0]);
                    int maxGroup = Int32.Parse(groupFields[1]);
                    if (maxGroup > 1 && curGroup != group)
                    {
                        continue;
                    }

                    groupStr = "";
                }

                Console.WriteLine($"{curLesson.AnfStd,2:00}-{curLesson.EndStd,2:00}{groupStr}: {curLesson.Lehrer,-5} {curLesson.UFachBez,-7} {curLesson.Raum}");
            }
        }

        public void PrintSA(int monat = -1)
        {
            foreach (DigikabuSA digikabuSa in _digikabuHandler.GetDates<DigikabuSA>(AISA.AnswerWriter, "termine/schulaufgaben", monat: monat))
            {
                Console.WriteLine($"{digikabuSa.Date.ToShortDateString()}: {digikabuSa.Info}");
            }
        }

        public void PrintDates(int idAbteilung = 1)
        {
            foreach (DigikabuDate digikabuSa in _digikabuHandler.GetDates<DigikabuDate>(AISA.AnswerWriter, idAbteilung: idAbteilung))
            {
                Console.WriteLine($"{digikabuSa.DateFrom:dd.MM.yyyy HH:mm}-{digikabuSa.DateTo:dd.MM.yyyy HH:mm}: {digikabuSa.Hinweis}");
            }
        }
    }
}