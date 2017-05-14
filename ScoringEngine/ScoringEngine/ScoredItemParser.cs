using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ScoringEngine
{
    public static class ScoredItemParser
    {
        public static List<ScoredItem> ParseFromFile(string filename)
        {
            JArray obj = JArray.Parse(File.ReadAllText(filename));
            List<ScoredItem> scoredItems = new List<ScoredItem>();
            foreach (var item in obj)
            {
                scoredItems.Add((ScoredItem)item.ToObject(Type.GetType(typeof(ScoredItem).Namespace + "." + item["type"].ToString() + "ScoredItem")));
            }
            return scoredItems;
        }

        public static void ScoreToFile(string filename, List<ScoredItem> items)
        {
            int totalScore = 0;
            int maxScore = 0;
            List<ScoredItem> scoredVulns = new List<ScoredItem>();
            List<ScoredItem> scoredPenalties = new List<ScoredItem>();

            foreach (var item in items)
            {
                if (item.CheckScored())
                {
                    if (item.IsPenalty)
                    {
                        scoredPenalties.Add(item);
                        totalScore -= item.Points;
                    }
                    else
                    {
                        scoredVulns.Add(item);
                        totalScore += item.Points;
                    }
                }
                if (!item.IsPenalty) maxScore += item.Points;
            }
            string templated = File.ReadAllText("ScoringTemplate.html");
            KeyValueList<string, string> toReplace = new KeyValueList<string, string>
            {
                { "TIME_STRING", DateTime.Now.ToString() },
                { "SCORE_STRING", $"{totalScore}/{maxScore}" },
                { "RANK_STRING", "1st place" },
                { "TEAM_COUNT_STRING", "of 12 teams" },
                { "COLOR_STRING", scoredPenalties.Count > 0 ? "red" : "blue" },
                { "PEN_SECTION", scoredPenalties.Count > 0 ? "<div class='col s12'><div class='card-panel red lighten-1 white-text'><h3 class='header'>Penalties</h3><h5>{{ PEN_STRING }}</h5><ul class='collection red'>{{ PEN_LIST }}</ul></div></div>" : "" },
                { "VULN_STRING", $"{scoredVulns.Count}/{items.Where(x => !x.IsPenalty).Count()} ({scoredVulns.Sum(x => x.Points)} pts)" },
                { "VULN_LIST", String.Join(Environment.NewLine, scoredVulns.Select(item=> $"<li class='collection-item blue-grey'>{item.Description} - {item.Points} pts</li>")) },
                { "PEN_STRING", $"{scoredPenalties.Count} (-{scoredPenalties.Sum(x => x.Points)} pts)" },
                { "PEN_LIST", String.Join(Environment.NewLine, scoredPenalties.Select(item=>$"<li class='collection-item red lighten-1'>{item.Description} - {item.Points} pts</li>"))},
            };

            foreach (var item in toReplace)
            {
                Regex regex = new Regex("{{\\s+" + item.Key + "\\s+}}");
                templated = regex.Replace(templated, item.Value);
            }
            File.WriteAllText(filename, templated);
        }

        public enum OS
        {
            Windows,
            Unix,
            MacOS,
            Other
        }

        public static OS GetOperatingSystem()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;
            switch (pid)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    return OS.Windows;
                case PlatformID.Unix:
                    return OS.Unix;
                case PlatformID.MacOSX:
                    return OS.MacOS;
                default:
                    return OS.Other;
            }
        }

        public class KeyValueList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
        {
            public void Add(TKey key, TValue value)
            {
                Add(new KeyValuePair<TKey, TValue>(key, value));
            }
        }
    }
}
