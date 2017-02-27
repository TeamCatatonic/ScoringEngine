using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace ScoringEngine
{
    public class RegistryScoredItem : ScoredItem
    {
        [JsonProperty("registryPath")]
        public string RegistryKey { get; protected set; }

        [JsonProperty("registryValue")]
        public string RegistryValue { get; protected set; }

        [JsonProperty("expectedValue")]
        public object ExpectedValue { get; protected set; }

        public override bool CheckScored()
        {
            return Registry.GetValue(RegistryKey, RegistryValue, null) == ExpectedValue;
        }
    }
}
