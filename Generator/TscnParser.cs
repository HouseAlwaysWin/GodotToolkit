using GodotToolkit.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GodotToolkit.Generator
{
    public class TscnParser
    {
        //public readonly List<TscnFileInfo> NodeList;
        //public readonly List<TscnFileInfo> ExtResList;
        public readonly TscnFileInfo TscnFile;
        public TscnParser(string path)
        {
            TscnFile = ParseTscn(path);
            //NodeList = GetNodeList(path);
            //ExtResList = GetExtResourceList(path);
        }

        //public string GetScriptPath()
        //{
        //    var path = ExtResList.FirstOrDefault(p => p.ExtResType == "Script");
        //    if (path != null)
        //    {
        //        return path.ExtResPath;
        //    }
        //    return string.Empty;
        //}

        public static Dictionary<string, string> GetTscnTitleProps(string line, string typePattern)
        {
            var typeInfo = Regex.Match(line, typePattern).Groups["value"].Value;
            var props = GetProps(typeInfo);
            return props;
        }

        public static Dictionary<string, string> GetProps(string line)
        {
            string propsPattern = @"(?<key>(\w+))\s*=\s*""(?<value>([^""]+))""|(?<key>(\w+))\s*=\s*\[(?<value>([^>]*))\]|(?<key>(\w+))\s*=\s*(?<value>([^*]+))";
            var propsDict = Regex.Matches(line, propsPattern)
                                    .Cast<Match>()
                                    .ToDictionary(q => q.Groups["key"].Value, q => q.Groups["value"].Value);
            return propsDict;
        }



        public static TscnFileInfo ParseTscn(string path)
        {
            TscnFileInfo tscnInfo = new TscnFileInfo();
            string scenePattern = @"\[gd_scene\s+(?<value>([^>]*))\]";
            string nodePattern = @"\[node\s+(?<value>([^>]*))\]";
            string extResourcePattern = @"\[ext_resource\s+(?<value>([^>]*))\]";
            string subResourcePattern = @"\[sub_resource\s+(?<value>([^>]*))\]";
            string connectionPattern = @"\[connection\s+(?<value>([^>]*))\]";
            var content = File.ReadAllLines(path);

            for (int i = 0; i < content.Length; i++)
            {
                var line = content[i];

                if (Regex.IsMatch(line, scenePattern))
                {
                    var props = GetTscnTitleProps(line, nodePattern);
                    var loadSteps = props.GetValueOrDefault("load_steps");
                    var format = props.GetValueOrDefault("format");
                    var uid = props.GetValueOrDefault("uid");
                    tscnInfo.LoadSteps = loadSteps;
                    tscnInfo.Format = format;
                    tscnInfo.UID = uid;
                }

                if (Regex.IsMatch(line, nodePattern))
                {
                    var props = GetTscnTitleProps(line, nodePattern);
                    var newNode = new TscnNode();
                    newNode.NodeName = props.GetValueOrDefault("name");
                    newNode.NodeType = props.GetValueOrDefault("type");
                    newNode.Parent = props.GetValueOrDefault("parent");
                    newNode.Index = props.GetValueOrDefault("index");
                    newNode.Instance = props.GetValueOrDefault("instance");
                    newNode.InstancePlaceHolder = props.GetValueOrDefault("instance_placeholder");
                    newNode.Owner = props.GetValueOrDefault("owner");
                    tscnInfo.Nodes.Add(newNode);
                }

                if (Regex.IsMatch(line, extResourcePattern))
                {
                    var props = GetTscnTitleProps(line, extResourcePattern);
                    var extRes = new TscnExtResource();
                    extRes.Id = props.GetValueOrDefault("id", string.Empty);
                    extRes.ResourceType = props.GetValueOrDefault("type", string.Empty);
                    extRes.Path = props.GetValueOrDefault("path", string.Empty);
                    extRes.UID = props.GetValueOrDefault("uid", string.Empty);
                    tscnInfo.ExtResource.Add(extRes);
                }

                if (Regex.IsMatch(line, subResourcePattern))
                {
                    var props = GetTscnTitleProps(line, subResourcePattern);
                    var subRes = new TscnSubResource();
                    subRes.Id = props.GetValueOrDefault("id", string.Empty);
                    subRes.Type = props.GetValueOrDefault("type", string.Empty);
                    tscnInfo.SubResource.Add(subRes);
                }

                if (Regex.IsMatch(line, connectionPattern))
                {
                    var props = GetTscnTitleProps(line, connectionPattern);
                    var connInfo = new TscnConnection();
                    connInfo.Signal = props.GetValueOrDefault("signal");
                    connInfo.From = props.GetValueOrDefault("from");
                    connInfo.To = props.GetValueOrDefault("to");
                    connInfo.Method = props.GetValueOrDefault("method");
                    tscnInfo.Connections.Add(connInfo);
                }


            }

            return tscnInfo;
        }

        //public static List<TscnFileInfo> GetExtResourceList(string path)
        //{
        //    string extResourcePattern = @"\[ext_resource\s+(?<value>([^>]*))\]";
        //    string scriptPath = string.Empty;
        //    List<TscnFileInfo> extResList = new List<TscnFileInfo>();

        //    using (StreamReader reader = new StreamReader(path))
        //    {
        //        string line;
        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            if (Regex.IsMatch(line, extResourcePattern))
        //            {
        //                var extResourceInfo = Regex.Match(line, extResourcePattern).Groups["value"].Value;
        //                var extKeyValue = Regex.Matches(extResourceInfo, @"(?<key>(\w+))=""(?<value>([^""]+))""|(?<key>(\w+))=\[(?<value>([^>]*))\]")
        //                            .Cast<Match>()
        //                            .ToDictionary(q => q.Groups["key"].Value, q => q.Groups["value"].Value);
        //                var extType = extKeyValue.ContainsKey("type") ? extKeyValue["type"] : string.Empty;
        //                var extPath = extKeyValue.ContainsKey("path") ? extKeyValue["path"] : string.Empty;
        //                var extId = extKeyValue.ContainsKey("id") ? extKeyValue["id"] : string.Empty;
        //                var newRes = new TscnFileInfo()
        //                {
        //                    FileFullPath = path,
        //                    ExtResType = extType,
        //                    ExtResPath = extPath,
        //                    ExtResId = extId
        //                };
        //                extResList.Add(newRes);

        //            }
        //        }
        //    }
        //    return extResList;
        //}


        //public static List<TscnFileInfo> GetNodeList(string path)
        //{
        //    string nodePattern = @"\[node\s+(?<value>([^>]*))\]";
        //    List<TscnFileInfo> nodeList = new List<TscnFileInfo>();
        //    using (StreamReader reader = new StreamReader(path))
        //    {
        //        string line;
        //        while ((line = reader.ReadLine()) != null)
        //        {

        //            //if (line.StartsWith("[node") && line.EndsWith("]"))
        //            if (Regex.IsMatch(line, nodePattern))
        //            {
        //                var nodeInfo = Regex.Match(line, nodePattern).Groups["value"].Value;
        //                var keyValue = Regex.Matches(nodeInfo, @"(?<key>(\w+))=""(?<value>([^""]+))""|(?<key>(\w+))=\[(?<value>([^>]*))\]")
        //                                    .Cast<Match>()
        //                                    .ToDictionary(q => q.Groups["key"].Value, q => q.Groups["value"].Value);
        //                var nodeName = keyValue.ContainsKey("name") ? keyValue["name"] : string.Empty;
        //                var nodeType = keyValue.ContainsKey("type") ? keyValue["type"] : string.Empty;
        //                var nodeParent = keyValue.ContainsKey("parent") ? keyValue["parent"] : string.Empty;
        //                var nodeGroups = keyValue.ContainsKey("groups") ? keyValue["groups"].Split(",").ToList() : new List<string>();
        //                var newNode = new TscnFileInfo()
        //                {
        //                    FileFullPath = path,
        //                    NodeName = nodeName,
        //                    NodeType = nodeType,
        //                    NodeParent = nodeParent,
        //                    NodeGroups = nodeGroups
        //                };
        //                nodeList.Add(newNode);
        //            }
        //        }
        //        return nodeList;
        //    }
        //}
    }
}
