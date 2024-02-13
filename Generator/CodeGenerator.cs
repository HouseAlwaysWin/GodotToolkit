using GodotToolkit.Models;
using Razor.Templating.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace GodotToolkit.Generator
{
    public class CodeGenerator
    {

        public static async Task<TemplateConfigs> BuildTemplate(string templateName, string currentVersion, TemplateConfigs templateConfig)
        {
            try
            {
                var templateContent = await RazorTemplateEngine.RenderAsync($@"{currentVersion}\{templateName}.cshtml", templateConfig);
                templateConfig.OriginalContent = templateContent;
                templateConfig.TemplateName = templateName;
                return templateConfig;
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Template Error: {currentVersion}\{templateName}", ex);
            }
        }
    }
}
