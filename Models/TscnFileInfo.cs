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

        public string NodeName { get; set; }
        public string NodeType { get; set; }
        public string NodeParent { get; set; }
        public List<string> NodeGroups { get; set; } = new List<string>();
    }
}
