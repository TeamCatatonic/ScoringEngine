using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoringEngine
{
    public class CompoundScoredItem : ScoredItem
    {
        public List<ScoredItem> ScoredItems { get; protected set; }
    }
}
