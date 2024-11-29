using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserResult
    {
        public string Username { get; set; }
        public string QuizId { get; set; }
        public string QuizTitle { get; set; }
        public int Score { get; set; }
        public DateTime AttemptDate { get; set; }
    }
}
