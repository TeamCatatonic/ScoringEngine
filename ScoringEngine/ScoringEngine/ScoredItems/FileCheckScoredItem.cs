using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoringEngine
{
    public class FileCheckScoredItem : FileScoredItem
    {
        public bool ShouldExist { get; protected set; }
    }
}
