using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class MatchQuestion:QuestionBase
    {
        public List<MatchPair> Pairs { get; set; } = new();
    }
}
