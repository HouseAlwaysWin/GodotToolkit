using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GodotToolkit.Extensions
{
    public static class StringExtensions
    {
        public static string ToHtmlString(this string builder)
        {
            builder = builder.Replace("&", "&amp;")
                             .Replace("<", "&lt;")
                             .Replace(">", "&gt;")
                             //.Replace("\"", "&quot;")

                             .ToString();
            var breakPattern = $@"(\n)|(\r\n)";
            //var breakPattern = $@"(\n+)";
            var tabPattern = $@"(\t)";

            builder = builder.Replace(" ", "&nbsp;");
            builder = Regex.Replace(builder, tabPattern, "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            builder = Regex.Replace(builder, breakPattern, "<br>");

            return builder;
        }
    }
}
