using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ScoringEngine
{
    public class FileCheckScoredItem : FileScoredItem
    {
        public bool ShouldExist { get; protected set; }

        public override bool CheckScored()
        {
            return File.Exists(FilePath) == ShouldExist;
        }
    }
}
