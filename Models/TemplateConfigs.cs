using GodotToolkit.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web;

namespace GodotToolkit.Models
{
    public class TemplateConfigs
    {
        public object InputParameter { get; set; } = new object();
        public string? OriginalContent { get; set; }
        public string? HtmlContent { get => HttpUtility.HtmlDecode(OriginalContent)?.ToHtmlString(); }
        public string? Content { get => HttpUtility.HtmlDecode(OriginalContent); }
        public string TemplateName { get; set; }
    }
}
