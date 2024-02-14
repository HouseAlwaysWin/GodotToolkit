using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GodotToolkit.Generator
{
    public class GodotParser
    {
        public Dictionary<string, string> AutoLoadInfo { get; set; }
        public GodotParser(string projectPath)
        {
            AutoLoadInfo = ReadAutoLoadInfo(projectPath);
        }

        public static Dictionary<string, string> GetProps(string line)
        {
            string propsPattern = @"(?<key>(\w+))\s*=\s*""(?<value>([^""]+))""|(?<key>(\w+))\s*=\s*\[(?<value>([^>]*))\]|(?<key>(\w+))\s*=\s*(?<value>([^*]+))";
            var propsDict = Regex.Matches(line, propsPattern)
                                    .Cast<Match>()
                                    .ToDictionary(q => q.Groups["key"].Value, q => q.Groups["value"].Value);
            return propsDict;
        }


        public static Dictionary<string, string> ReadAutoLoadInfo(string projPath)
        {
            var content = File.ReadAllLines($"{projPath}/project.godot");
            var dict = new Dictionary<string, string>();

            for (int i = 0; i < content.Length; i++)
            {
                var line = content[i];
                if (Regex.IsMatch(line, @"\s*\[autoload\]\s*"))
                {
                    i++;
                    while (!string.IsNullOrWhiteSpace(line))
                    {
                        if (i == content.Length - 1) break;
                        i++;
                        line = content[i];
                        var propsDict = GetProps(line);
                        if (propsDict.Count > 0)
                        {
                            foreach (var prop in propsDict)
                            {
                                dict.Add(prop.Key, prop.Value);
                            }
                        }

                    }

                }
            }

            return dict;
        }
    }
}
