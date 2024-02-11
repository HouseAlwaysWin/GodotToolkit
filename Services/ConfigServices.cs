using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;

namespace GodotToolkit.Services
{
    public class ConfigServices : IConfigServices
    {
        private JsonNode _config;
        public string ConfigName { get; set; }
        public string ConfigPath { get; set; }

        public ConfigServices()
        {
            ConfigName = "configs.json";
            ConfigPath = Path.Combine(AppContext.BaseDirectory, ConfigName);
        }

        public void ClearConfig()
        {
            File.WriteAllText(ConfigPath, "{}");
        }

        /// <summary>
        /// 取得Config物件
        /// </summary>
        /// <returns></returns>
        public JsonNode GetConfig()
        {
            string config = string.Empty;
            try
            {
                if (!File.Exists(ConfigPath))
                {
                    File.WriteAllText(ConfigPath, "{}");
                }

                config = File.ReadAllText(ConfigPath);
                _config = JsonNode.Parse(config) ?? new JsonObject();
            }
            catch (Exception ex)
            {
                File.WriteAllText(ConfigPath, "{}");
                config = File.ReadAllText(ConfigPath);
                _config = JsonNode.Parse(config) ?? new JsonObject();
            }
            return _config;
        }

        /// <summary>
        /// 更新Config
        /// </summary>
        /// <param name="config"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateConfig(JsonNode config)
        {
            try
            {
                if (!File.Exists(ConfigPath))
                {
                    File.Create(ConfigPath);
                    File.WriteAllText(ConfigPath, "{}");
                }

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 使用不進行跳脫的編碼器
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(config, options);
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing app settings | {ex.Message}", ex);
            }
        }

        public void SetDefaultNavPath(string navPath)
        {
            _config["DefaultPath"] = navPath;
            UpdateConfig(_config);
        }


    }
}
