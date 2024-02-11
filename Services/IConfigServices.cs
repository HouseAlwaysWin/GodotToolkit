using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace GodotToolkit.Services
{
    public interface IConfigServices
    {
        void UpdateConfig(JsonNode config);
        JsonNode GetConfig();
        public void ClearConfig();
        void SetDefaultNavPath(string navPath);

    }
}
