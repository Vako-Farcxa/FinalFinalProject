using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Quiz
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<Question> Questions { get; set; }
        public string Creator { get; set; }
    }
}
