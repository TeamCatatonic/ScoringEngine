using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoringEngine
{
    public class FileContainsScoredItem : FileScoredItem
    {
        public string RegexCheck { get; protected set; }
    }
}
