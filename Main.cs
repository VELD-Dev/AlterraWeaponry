using System.Linq;
using System.Reflection;
using HarmonyLib;
using QModManager.Utility;
using QModManager.API.ModLoading;

using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using SMLHelper.V2.Handlers;

using Logger = QModManager.Utility.Logger;

namespace VELDsAlterraWeaponry
{

    [QModCore]
    public class AlterraWeaponry
    {
        internal static ModConfigs Config { get; private set; }
        [QModPatch]
        public static void Patch()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var modName = ($"velddev_{assembly.GetName().Name}");
            //var explosiveTorpedo = new ExplosiveTorpedo();
            Logger.Log(Logger.Level.Info, $"Patching {modName}...");

            Harmony harmony = new Harmony(modName);
            harmony.PatchAll(assembly);

            Config = OptionsPanelHandler.Main.RegisterModOptions<ModConfigs>();

            new ExplosiveTorpedoItem().Patch();
            new CoalItem().Patch();
            new GunpowderItem().Patch();
            new PrawnSelfDefenseModule().Patch();

            Logger.Log(Logger.Level.Info, "Patched successfully.");
        }
        public static void OnLethalItemCrafted()
        {
        }
    }
}
