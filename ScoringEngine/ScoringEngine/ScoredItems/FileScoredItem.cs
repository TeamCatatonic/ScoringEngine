using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ScoringEngine
{
    public abstract class FileScoredItem : ScoredItem
    {
        [JsonProperty("filePath")]
        public string FilePath { get; protected set; }
    }
}
