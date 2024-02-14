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
        public readonly TscnFileInfo TscnFile;
        public TscnParser(string path)
        {
            TscnFile = ParseTscn(path);
        }

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
                    newNode.NodeName = props.GetValueOrDefault("name", string.Empty);
                    newNode.NodeType = props.GetValueOrDefault("type", string.Empty);
                    newNode.Parent = props.GetValueOrDefault("parent", string.Empty);
                    newNode.Index = props.GetValueOrDefault("index", string.Empty);
                    newNode.Instance = props.GetValueOrDefault("instance", string.Empty);
                    newNode.InstancePlaceHolder = props.GetValueOrDefault("instance_placeholder", string.Empty);
                    newNode.Owner = props.GetValueOrDefault("owner", string.Empty);
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
                    connInfo.Signal = props.GetValueOrDefault("signal", string.Empty);
                    connInfo.From = props.GetValueOrDefault("from", string.Empty);
                    connInfo.To = props.GetValueOrDefault("to", string.Empty);
                    connInfo.Method = props.GetValueOrDefault("method", string.Empty);
                    tscnInfo.Connections.Add(connInfo);
                }


            }

            return tscnInfo;
        }
    }
}
