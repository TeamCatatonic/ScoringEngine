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
            Dictionary<string, string> toReplace = new Dictionary<string, string>()
            {
                { "TIME_STRING", DateTime.Now.ToString() },
                { "SCORE_STRING", $"{totalScore}/{maxScore}" },
                { "VULN_STRING", $"{scoredVulns.Count}/{items.Where(x => !x.IsPenalty).Count()} ({scoredVulns.Sum(x => x.Points)} pts)" },
                { "VULN_LIST", String.Join(Environment.NewLine, (from item in scoredVulns select $"<li class='collection-item'>{item.Description} - {item.Points} pts</li>"))},
                { "PEN_STRING", $"{scoredPenalties.Count} ({scoredPenalties.Sum(x => x.Points)} pts)" },
                { "PEN_LIST", String.Join(Environment.NewLine, (from item in scoredPenalties select $"<li class='collection-item'>{item.Description} - {item.Points} pts</li>"))},
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
    }
}
