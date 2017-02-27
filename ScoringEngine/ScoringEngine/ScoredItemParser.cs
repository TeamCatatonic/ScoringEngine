using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;

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
            List<string> scoredSuccessful = new List<string>();
            List<string> scoredPenalties = new List<string>();

            foreach (var item in items)
            {
                if (item.CheckScored())
                {
                    if (item.IsPenalty) scoredPenalties.Add(item.Description);
                    else scoredSuccessful.Add(item.Description);
                    totalScore += item.Points;
                }
                maxScore += item.Points;
            }
            string output = $"<html><body><h1>Score: {totalScore}/{maxScore}</h1><h2>Vulnerabilities: {scoredSuccessful.Count}/{items.Count}</h2><p>{String.Join(Environment.NewLine, scoredSuccessful)}</p><p style='color: red;'>{String.Join(Environment.NewLine, scoredPenalties)}</p></body></html>";
            File.WriteAllText(filename, output);
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
