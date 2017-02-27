using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace ScoringEngine
{
    public class FileContainsScoredItem : FileScoredItem
    {
        public string RegexPattern { get; protected set; }
        public bool ShouldMatch { get; protected set; }

        public override bool CheckScored()
        {
            return new Regex(RegexPattern).IsMatch(File.ReadAllText(FilePath)) == ShouldMatch;
        }
    }
}
