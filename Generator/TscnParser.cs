using GodotToolkit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GodotToolkit.Generator
{
    public class TscnParser
    {
        List<TscnFileInfo> NodeList;
        public TscnParser(string path)
        {
            NodeList = GetNodeList(path);
        }

        public static List<TscnFileInfo> GetNodeList(string path)
        {
            string nodePattern = @"\[node\s+(?<value>([^>]*))\]";
            List<TscnFileInfo> nodeList = new List<TscnFileInfo>();
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {

                    //if (line.StartsWith("[node") && line.EndsWith("]"))
                    if (Regex.IsMatch(line, nodePattern))
                    {
                        var nodeInfo = Regex.Match(line, nodePattern).Groups["value"].Value;
                        var keyValue = Regex.Matches(nodeInfo, @"(?<key>(\w+))=""(?<value>([^""]+))""|(?<key>(\w+))=\[(?<value>([^>]*))\]")
                                            .Cast<Match>()
                                            .ToDictionary(q => q.Groups["key"].Value, q => q.Groups["value"].Value);
                        var nodeName = keyValue.ContainsKey("name") ? keyValue["name"] : string.Empty;
                        var nodeType = keyValue.ContainsKey("type") ? keyValue["type"] : string.Empty;
                        var nodeParent = keyValue.ContainsKey("parent") ? keyValue["parent"] : string.Empty;
                        var nodeGroups = keyValue.ContainsKey("groups") ? keyValue["groups"].Split(",").ToList() : new List<string>();
                        var newNode = new TscnFileInfo()
                        {
                            FileFullPath = path,
                            NodeName = nodeName,
                            NodeType = nodeType,
                            NodeParent = nodeParent,
                            NodeGroups = nodeGroups
                        };
                        nodeList.Add(newNode);
                    }
                }
                return nodeList;
            }
        }
    }
}
