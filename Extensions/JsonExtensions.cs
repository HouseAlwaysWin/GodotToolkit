using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace GodotToolkit.Extensions
{
    public static class JsonExtensions
    {
        public static bool ToBoolean(this JsonNode obj)
        {
            if (obj == null) return false;
            bool result = false;
            bool.TryParse(obj.ToString(), out result);
            return result;
        }

        public static int ToInt(this JsonNode obj)
        {
            int result = 0;
            int.TryParse(obj.ToString(), out result);
            return result;
        }

        public static decimal ToDecimal(this JsonNode obj)
        {
            decimal result = 0;
            decimal.TryParse(obj.ToString(), out result);
            return result;
        }

        public static float ToFloat(this JsonNode obj)
        {
            float result = 0;
            float.TryParse(obj.ToString(), out result);
            return result;
        }

        public static JsonNode ToJsonNode(this JsonNode node, string key)
        {
            return node?[key]?.CopyNode() ?? (JsonNode)string.Empty.ToString();
        }

        public static JsonObject ToJsonObject(this JsonNode node, string key)
        {
            return node?[key]?.CopyNode()?.AsObject() ?? new JsonObject();
        }

        public static string ToValue(this JsonNode node, string key)
        {
            return node?[key]?.ToString() ?? "";
        }

        public static dynamic ToValue<T>(this JsonNode node, string key)
        {
            if (typeof(T) == typeof(string))
            {
                return node?[key]?.ToString() ?? "";
            }

            if (typeof(T) == typeof(bool))
            {
                return node?[key]?.ToBoolean() ?? false;
            }

            if (typeof(T) == typeof(int))
            {
                return node?[key]?.ToInt() ?? 0;
            }

            if (typeof(T) == typeof(float))
            {
                return node?[key]?.ToFloat() ?? 0;
            }

            if (typeof(T) == typeof(decimal))
            {
                return node?[key]?.ToDecimal() ?? 0;
            }

            if (node?[key] != null || typeof(T).IsClass)
            {
                try
                {
                    T result = JsonSerializer.Deserialize<T>(node?[key]);
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return default(T);
        }

        public static JsonNode? CopyNode(this JsonNode obj)
        {
            try
            {
                if (obj != null)
                {
                    return JsonNode.Parse(obj.ToJsonString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonObject();
        }

        public static JsonArray ToJsonArray(this JsonNode node)
        {
            JsonArray result = new JsonArray();
            if (node != null && node is JsonArray)
            {
                foreach (JsonNode data in node?.AsArray())
                {
                    if (data != null)
                    {
                        (result as JsonArray)?.Add(data.CopyNode());
                    }
                }
            }
            else if (node != null && node is JsonObject)
            {
                result.Add(node.CopyNode());
            }
            return result;
        }

        public static JsonArray ToJsonArray(this JsonNode node, string name)
        {
            JsonArray result = new JsonArray();
            var curNode = node.ToJsonNode(name);
            if (curNode != null && curNode is JsonArray)
            {
                foreach (JsonNode data in curNode?.AsArray())
                {
                    if (data != null)
                    {
                        (result as JsonArray)?.Add(data.CopyNode());
                    }
                }
            }
            else if (curNode != null && curNode is JsonObject)
            {
                result.Add(curNode.CopyNode());
            }
            return result;
        }

    }
}
