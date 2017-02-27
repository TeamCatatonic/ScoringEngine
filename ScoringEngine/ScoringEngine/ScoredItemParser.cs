using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ScoringEngine
{
    public static class ScoredItemParser
    {
        public static List<ScoredItem> ParseFromFile(string filename)
        {
            JObject obj = JObject.Parse(File.ReadAllText(filename));
            List<ScoredItem> scoredItems = new List<ScoredItem>();
            foreach (var item in obj.AsJEnumerable())
            {
                scoredItems.Add((ScoredItem)typeof(JsonConvert)
                    .GetMethod("DeserializeObject", System.Reflection.BindingFlags.Static)
                    .MakeGenericMethod(Type.GetType(item["type"].ToString()))
                    .Invoke(null, new[] { item }));
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
    }
}
