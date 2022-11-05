using System.Linq;
using System.Reflection;
using HarmonyLib;
using QModManager.Utility;
using QModManager.API.ModLoading;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

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
            var prawnDefUpgrade = new PrawnSelfDefenseModule();
            Logger.Log(Logger.Level.Info, $"Patching {modName}...");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), modName);

            Config = OptionsPanelHandler.Main.RegisterModOptions<ModConfigs>();
            PrawnLaserArm.AddPDAEntry();
            PrawnSelfDefenseModule.AddPDAEntry();
            PDA_Patch.AddPDAEntry("AWModInfo", "Meta", hidden: true, unlocked: true);
            MessagesHandler.AddMessageEntry("presentation", Path.Combine(AssetFolder, "xenoworx_pda_presentation.mp3"));
            MessagesHandler.AddMessageEntry("first_lethal", Path.Combine(AssetFolder, "first_lethal_weapon_message.mp3"));

            new CoalItem().Patch();
            new BlackPowderItem().Patch();
            new AdvancedRefractor().Patch();
            explosiveTorpedo.Patch();
            prawnDefUpgrade.Patch();
            prawnLaserArm.Patch();

            

            OnPickup.techTypes.Add(explosiveTorpedo.TechType);
            OnPickup.techTypes.Add(prawnLaserArm.TechType);

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
                Logger.Log(Logger.Level.Info, "START EVENT - Should play an audio right now !!");
                PDASounds.queue.Play("presentation", SoundHost.PDA, true, subtitles: "Subtitles_AWPresentation");
                Logger.Log(Logger.Level.Info, "Presentation audio has played.");
            }
        }
    }

    public class OnPickup : MonoBehaviour
    {
        public static List<TechType> techTypes = new List<TechType>();

        public void OnExamine()
        {
            Logger.Log(Logger.Level.Info, "Should try to see if a picked-up item is registered in techTypes", showOnScreen: true);
            foreach(var techType in techTypes)
            {
                if (KnownTech.Contains(techType)) return;
            }
            Logger.Log(Logger.Level.Info, "Yes, it should play an audio right now !!", showOnScreen: true);
            PDASounds.queue.Play("first_lethal", SoundHost.PDA, true, subtitles: "Subtitles_AWFirstLethal");
            PDAEncyclopedia.Reveal("ExplosiveTorpedo", false);
        }
    }

    [HarmonyPatch(typeof(Vehicle))]
    internal class ExosuitPatches
    {
        [HarmonyPatch("OnUpgradeModuleUse")]
        public static void Postfix(TechType techType, Vehicle __instance)
        {
            if (techType == PrawnSelfDefenseModule.thisTechType)
            {
                Logger.Log(Logger.Level.Debug, "Input received", showOnScreen: true);
                if (!__instance.TryGetComponent(out PrawnSelfDefenseModule.ShockFunctionality defenseMono))
                    return;
                defenseMono.Use(__instance, __instance.gameObject);
            }
        }
    }

    [HarmonyPatch(typeof(VehicleUpgradeConsoleInput))]
    internal class UpgradesPatch
    {
        [HarmonyPatch("OnUpgradeModuleChange")]
        public static void Postfix(VehicleUpgradeConsoleInput __instance, TechType techType, bool added)
        {
            if(techType == PrawnSelfDefenseModule.thisTechType)
            {

            }
        }
    }
}
