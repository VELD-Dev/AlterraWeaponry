namespace VELDsAlterraWeaponry.patches;

[HarmonyPatch(typeof(Vehicle))]
internal class Vehicle_OnUpgradeModuleUse_Patch
{
    [HarmonyPatch("OnUpgradeModuleUse")]
    public static void Postfix(TechType techType, Vehicle __instance)
    {
        if (techType == PrawnSelfDefenseModule.thisTechType)
        {
            AlterraWeaponry.logger.LogInfo("Input received");
            if (!__instance.TryGetComponent(out PrawnSelfDefenseModule.ShockFunctionality defenseMono))
                return;
            defenseMono.Use(__instance, __instance.gameObject);
        }
    }
}
