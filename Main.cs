using System.Linq;
using System.Reflection;
using HarmonyLib;
using QModManager.Utility;
using QModManager.API.ModLoading;
using System.IO;

using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using SMLHelper.V2.Handlers;
using UnityEngine;

using Logger = QModManager.Utility.Logger;

namespace VELDsAlterraWeaponry
{

    [QModCore]
    public class AlterraWeaponry
    {
        internal static ModConfigs Config { get; private set; }
        internal static string AssetFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");
        [QModPatch]
        public static void Patch()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var modName = ($"VELDs_{assembly.GetName().Name}");
            var explosiveTorpedo = new ExplosiveTorpedoItem();
            var prawnLaserArm = new PrawnLaserArm();
            Logger.Log(Logger.Level.Info, $"Patching {modName}...");

            Harmony harmony = new Harmony(modName);
            harmony.PatchAll(typeof(GameSettingsPatch));

            Config = OptionsPanelHandler.Main.RegisterModOptions<ModConfigs>();
            PrawnLaserArm.AddPDAEntry();
            PrawnSelfDefenseModule.AddPDAEntry();
            MessagesHandler.AddMessageEntry("presentation", Path.Combine(AssetFolder, "xenoworx_pda_presentation.mp3"));
            MessagesHandler.AddMessageEntry("first_lethal", Path.Combine(AssetFolder, "first_lethal_weapon_message.mp3"));

            new CoalItem().Patch();
            new BlackPowderItem().Patch();
            new AdvancedRefractor().Patch();
            explosiveTorpedo.Patch();
            new PrawnSelfDefenseModule().Patch();
            prawnLaserArm.Patch();

            OnPickup.techTypes.AddItem(explosiveTorpedo.TechType);
            OnPickup.techTypes.AddItem(prawnLaserArm.TechType);

            LanguagesHandler.LanguagePatch();
            Logger.Log(Logger.Level.Info, "Patched successfully.");
        }

        [HarmonyPatch(typeof(GameSettings), nameof(GameSettings.SaveAsync))]
        internal class GameSettingsPatch
        {
            public static void Postfix()
            {
                LanguagesHandler.LanguagePatch();
            }
        }

        [HarmonyPatch(typeof(Player))]
        internal class PlayerPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch("Start")]
            public static void StartPatch()
            {
                PDASounds.queue.Play("presentation", SoundHost.PDA, true, subtitles: "Subtitles_AWPresentation");
            }
        }
    }

    public class OnPickup : MonoBehaviour
    {
        public static TechType[] techTypes;

        public void OnExamine()
        {
            foreach(var techType in techTypes)
            {
                if (KnownTech.Contains(techType)) return;
            }
            PDASounds.queue.Play("first_lethal", SoundHost.PDA, true, subtitles: "Subtitles_AWFirstLethal");
            PDAEncyclopedia.Add("ExplosiveTorpedo", false, true);
        }
    }
}
