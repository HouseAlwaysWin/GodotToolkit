using GodotToolkit.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GodotToolkit.Services
{
    public class LocalizationService
    {
        public string ConfigPath;
        public string CurrentLang = "en";
        public LangInfo Langs = new LangInfo();

        private readonly ISubject<LangInfo> _langSubject = new Subject<LangInfo>();
        public IObservable<LangInfo> LangsState => _langSubject;

        public LocalizationService()
        {
            ConfigPath = $@"{AppContext.BaseDirectory}/Localization/Langs";
            SetLocalization(CurrentLang);
        }

        public LangInfo InitLang(Action<LangInfo> stateChanged)
        {
            this.LangsState.Subscribe(stateChanged);
            return this.Langs;
        }



        public LangInfo SetLocalization(string code)
        {
            if (!Directory.Exists(ConfigPath))
            {
                Directory.CreateDirectory(ConfigPath);
            }

            var filePath = Path.Combine(ConfigPath, $"lang-{code}.json");
            string content = string.Empty;

            try
            {
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, "{}");
                }
                content = File.ReadAllText(filePath);
                Langs.UpdateLang(JsonNode.Parse(content)?.AsObject() ?? new JsonObject());
                CurrentLang = code;
            }
            catch (Exception ex)
            {
                File.WriteAllText(filePath, "{}");
                content = File.ReadAllText(ConfigPath);
                Langs.UpdateLang(JsonNode.Parse(content)?.AsObject() ?? new JsonObject());
            }
            _langSubject.OnNext(Langs);
            return Langs;
        }

    }
}
