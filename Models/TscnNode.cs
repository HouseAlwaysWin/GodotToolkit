using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotToolkit.Models
{
    public class TscnNode
    {
        public string NodeType { get; set; }
        public string NodeName { get; set; }
        public string Parent { get; set; }
        public string Index { get; set; }
        public List<string> Groups { get; set; }
        public string Instance { get; set; }
        public string InstancePlaceHolder { get; set; }
        public string Owner { get; set; }
    }
}
