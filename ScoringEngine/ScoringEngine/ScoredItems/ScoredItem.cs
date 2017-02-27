using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoringEngine
{
    public abstract class ScoredItem
    {
        public int Points { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public bool Penalty { get; protected set; }

        public abstract bool CheckScored();
    }
}
