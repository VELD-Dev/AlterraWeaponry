using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

using SMLHelper.V2.Handlers;
using Logger = QModManager.Utility.Logger;

namespace VELDsAlterraWeaponry
{
    namespace LocalizationHandler
    {
        [XmlRoot("LocalizationPackages")]
        public class LocalizationPackages
        {
            [XmlElement("LocalizationPackage")]
            public LocalizationPackage[] Localizations;
        }

        /// <summary>
        /// LocalizationPackage is a Localization Package ref - 
        /// </summary>
        public class LocalizationPackage
        {
            [XmlAttribute("Lang")]
            public string Lang;

            [XmlElement("Text")]
            public Text[] Texts;
        }

        public class Text
        {
            [XmlAttribute]
            public string key;
            [XmlText]
            public string value;
        }
    }

    class LanguagesHandler
    {
        private static string ModPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string filename = "Localizations.xml";
        public static void LanguagePatch()
        {
            Logger.Log(Logger.Level.Info, "Starting patching the languages !");
            XmlSerializer serializer = new XmlSerializer(typeof(LocalizationHandler.LocalizationPackages));

            FileStream fs = new FileStream(Path.Combine(ModPath, filename), FileMode.Open);
            LocalizationHandler.LocalizationPackages lps;

            Logger.Log(Logger.Level.Info, Language.main.GetCurrentLanguage());

            lps = (LocalizationHandler.LocalizationPackages)serializer.Deserialize(fs);

            foreach (LocalizationHandler.LocalizationPackage lockalizationpack in lps.Localizations) Logger.Log(Logger.Level.Info, lockalizationpack.Lang);
            Logger.Log(Logger.Level.Info, "All LPs logged.");

            foreach(LocalizationHandler.Text text in lps.Localizations.Single(lp => lps.Localizations.Any(lp1 => lp1.Lang == Language.main.GetCurrentLanguage()) ? lp.Lang == Language.main.GetCurrentLanguage() : lp.Lang == Language.defaultLanguage).Texts)
            {
                Logger.Log(Logger.Level.Info, $"Checking string, key {text.key}");
                if (Language.main.Get(text.key) != null)
                {
                    LanguageHandler.SetLanguageLine(text.key, text.value);
                    Logger.Log(Logger.Level.Info, $"Patched key {text.key} with text '{(text.value.Length > 50 ? text.value.Substring(50) : text.value)}'");
                } 
                else
                {
                    Logger.Log(Logger.Level.Warn, $"Key {text.key} does not reference any key in game. Please check the case.");
                }
            }
            Logger.Log(Logger.Level.Info, "Language patching done.");
        }
    }
}
