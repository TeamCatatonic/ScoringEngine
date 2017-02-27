﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ScoringEngine
{
    public abstract class FileScoredItem : ScoredItem
    {
        public string FilePath { get; protected set; }
    }
}