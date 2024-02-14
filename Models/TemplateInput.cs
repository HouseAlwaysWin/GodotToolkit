using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodotToolkit.Generator;

namespace GodotToolkit.Models
{
    public class TemplateInput
    {
        public readonly TscnParser TscnParser;
        public readonly GodotParser GodotParser;

        public TemplateInput(string projectPath, string tscnPath)
        {
            TscnParser = new TscnParser(tscnPath);
            GodotParser = new GodotParser(projectPath);
        }
    }
}
