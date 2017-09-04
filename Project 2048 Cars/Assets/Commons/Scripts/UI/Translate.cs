using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Commons.Lang;

namespace Commons.UI
{
    class TranslateUIProvider
    {
        public string PreferredLanguage = "en";
        public string FallbackLanguage = "en";
        public string FilesLoaderPrefix = "i18n/locale-";
        TranslateProvider core;

        public void Initialize()
        {
            // TODO load preference
            // LoadISO();
            core = new TranslateProvider();
            core.PreferredLanguage = PreferredLanguage;
            core.FallbackLanguage = FallbackLanguage;
            core.Translations(PreferredLanguage, LoadFile(PreferredLanguage));
            core.Translations(FallbackLanguage, LoadFile(FallbackLanguage));
        }

        string LoadFile(string language)
        {
            string res;
            string path = FilesLoaderPrefix + language;
            TextAsset txt = Resources.Load<TextAsset>(path);
            if (txt != null)
            {
                res = txt.text;
            }
            else
            {
                res = null;
            }
            return res;
        }

        public void Apply(GameObject go, bool includeInactive = false, int depth = 10)
        {
            if (depth <= 0)
                return;
            string name = go.name.ToLower();
            string key;
            if (name.Contains("i18n="))
            {
                string[] res = Regex.Split(name, "i18n=");
                res = res[1].Split(' ');
                key = res[0].ToUpper();
                Apply(go, key);
            }
            if (go.transform.childCount > 0)
                for (int i = 0; i < go.transform.childCount; i++)
                    Apply(
                        go.transform.GetChild(i).gameObject,
                        includeInactive, depth - 1);
        }

        void Apply(GameObject go, string key)
        {
            string value = null;
            Text text;
            if (key == null)
                return;
            if (go.GetComponent<Text>() != null)
            {
                value = core.Translate(key);
                if (value == null)
                    return;
                text = go.GetComponent<Text>();
                text.text = value;
            }
        }
    }

    public static class GameObjectExtensions
    {
        static Dictionary<SystemLanguage, string> isos = Initialize();
        static Dictionary<SystemLanguage, string> Initialize()
        {
            Dictionary<SystemLanguage, string> isos = new Dictionary<SystemLanguage, string>();
            isos.Add(SystemLanguage.English, "en");
            isos.Add(SystemLanguage.French, "fr");
            isos.Add(SystemLanguage.Spanish, "es");
            isos.Add(SystemLanguage.Chinese, "es");
            return isos;
        }

        public static string ISO(this SystemLanguage systemLanguage)
        {
            return isos[systemLanguage];
        }
    }
}
