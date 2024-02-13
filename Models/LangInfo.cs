using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace GodotToolkit.Models
{
    public class LangInfo
    {
        private JsonObject _lang = new JsonObject();

        public void UpdateLang(JsonObject lang)
        {
            _lang = lang;
        }

        public string this[string index]
        {
            get
            {
                if (!_lang.ContainsKey(index) || _lang[index] == null)
                {
                    return index;
                }
                return _lang[index].ToString();
            }
        }

    }
}
