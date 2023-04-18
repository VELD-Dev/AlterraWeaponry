using System.Linq;
using System.Xml;
using System.Xml.Serialization;

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

    public class LanguagesHandler
    {
        private static string ModPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string filename = "Localizations.xml";
        public static void LanguagePatch()
        {
            AlterraWeaponry.logger.LogInfo("Starting patching the languages !");
            XmlSerializer serializer = new(typeof(LocalizationHandler.LocalizationPackages));

            FileStream fs = new(Path.Combine(ModPath, filename), FileMode.Open);
            LocalizationHandler.LocalizationPackages lps;

            AlterraWeaponry.logger.LogInfo(Language.main.GetCurrentLanguage());

            lps = (LocalizationHandler.LocalizationPackages)serializer.Deserialize(fs);

            foreach (LocalizationHandler.LocalizationPackage localizationpack in lps.Localizations)
                AlterraWeaponry.logger.LogInfo(localizationpack.Lang);
            AlterraWeaponry.logger.LogInfo("All LPs logged.");

            foreach (LocalizationHandler.Text text in lps.Localizations.Single(lp => lps.Localizations.Any(lp1 => lp1.Lang == Language.main.GetCurrentLanguage()) ? lp.Lang == Language.main.GetCurrentLanguage() : lp.Lang == Language.defaultLanguage).Texts)
            {
                AlterraWeaponry.logger.LogInfo($"Checking string, key {text.key}");
                if (Language.main.Get(text.key) != null)
                {
                    LanguageHandler.SetLanguageLine(text.key, text.value);
                    AlterraWeaponry.logger.LogInfo($"Patched key {text.key} with text '{(text.value.Length > 50 ? text.value.Substring(50) : text.value)}'");
                }
                else
                {
                    AlterraWeaponry.logger.LogInfo($"Key {text.key} does not reference any key in game. Please check the case.");
                }
            }
            AlterraWeaponry.logger.LogInfo("Language patching done.");
        }
    }
}
