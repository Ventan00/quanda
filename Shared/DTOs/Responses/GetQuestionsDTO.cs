using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanda.Shared.DTOs.Responses
{
    public class GetQuestionsDTO
    {
        public int IdQuestion { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public int Views { get; set; }
        public bool IsFinished { get; set; }
        public bool IsModified { get; set; }
        public int IdUser { get; set; }
        public string Nickname { get; set; }
        public string Avatar { get; set; }
        public int AnswersCount { get; set; }
        public List<string> Categories { get; set; }
    }
}
