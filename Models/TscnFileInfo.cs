using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotToolkit.Models
{
    public class TscnFileInfo
    {
        public string FileName { get; set; }
        public string FileFullPath { get; set; }
        public string FileFolderName { get; set; }

        public string LoadSteps { get; set; }
        public string Format { get; set; }
        public string UID { get; set; }

        public List<TscnNode> Nodes = new List<TscnNode>();
        public List<TscnExtResource> ExtResource = new List<TscnExtResource>();
        public List<TscnSubResource> SubResource = new List<TscnSubResource>();
        public List<TscnConnection> Connections = new List<TscnConnection>();

    }
}
