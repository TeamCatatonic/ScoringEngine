using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ScoringEngine
{
    public class FileContainsScoredItem : FileScoredItem
    {

        [JsonProperty("regexPattern")]
        public string RegexPattern { get; protected set; }

        public override bool CheckScored()
        {
            throw new NotImplementedException();
        }
    }
}
