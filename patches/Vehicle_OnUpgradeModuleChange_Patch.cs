namespace VELDsAlterraWeaponry.patches;

[HarmonyPatch(typeof(Vehicle))]
internal class Vehicle_OnUpgradeModuleChange_Patch
{
    [HarmonyPatch("OnUpgradeModuleChange")]
    public static void Postfix(Vehicle __instance, TechType techType, bool added)
    {
        if (techType == PrawnSelfDefenseModule.thisTechType)
        {
            AlterraWeaponry.logger.LogInfo("Module Self Defense for Prawn loaded in.");
        }
    }
}
