using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoringEngine
{
    public abstract class ScoredItem
    {
        [JsonProperty("points")]
        public int Points { get; protected set; }

        [JsonProperty("name")]
        public string Name { get; protected set; }

        [JsonProperty("description")]
        public string Description { get; protected set; }

        [JsonProperty("isPenalty")]
        public bool IsPenalty { get; protected set; }

        public abstract bool CheckScored();
    }
}
