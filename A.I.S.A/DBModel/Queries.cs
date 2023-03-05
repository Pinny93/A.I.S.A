using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A.I.S.A_.DBModel
{
    public class Queries
    {
        public Guid Id { get; set; }

        public DateTime RequestTimestamp { get; } = DateTime.Now;

        public string Prompt { get; set; } = String.Empty;

        public string Answer { get; set; } = String.Empty;

        public string AnswerFull { get; set; } = String.Empty;
    }
}