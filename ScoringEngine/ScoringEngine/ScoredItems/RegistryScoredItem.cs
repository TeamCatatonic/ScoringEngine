using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ScoringEngine
{
    public class RegistryScoredItem : ScoredItem
    {
        public string RegistryKey { get; protected set; }
        public string RegistryValue { get; protected set; }
        public object ExpectedValue { get; protected set; }

        public override bool CheckScored()
        {
            return Registry.GetValue(RegistryKey, RegistryValue, null) == ExpectedValue;
        }
    }
}
