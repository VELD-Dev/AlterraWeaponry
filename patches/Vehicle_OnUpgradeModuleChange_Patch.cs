namespace VELDsAlterraWeaponry.patches;

[HarmonyPatch(typeof(Vehicle))]
internal class Vehicle_OnUpgradeModuleChange_Patch
{
    [HarmonyPatch("OnUpgradeModuleChange")]
    public static void Postfix(Vehicle __instance, TechType techType, bool added)
    {
        if (techType == PrawnSelfDefenseModule.thisTechType)
        {
            if (!added)
            {
                //__instance.gameObject.TryGetComponent<PrawnSelfDefenseModule.ShockFunctionality>(out PrawnSelfDefenseModule.ShockFunctionality defenseMono);
                //defenseMono.gameObject.SetActive(false);
                AlterraWeaponry.logger.LogInfo("Module Self Defense for Prawn unloaded.");
                Subtitles.Add("<color=yellow>PRAWN:</color> Self defense module for PRAWN unarmed. Caution advised.");
                return;
            }
            AlterraWeaponry.logger.LogInfo("Module Self Defense for Prawn loaded in.");
            __instance.gameObject.EnsureComponent<PrawnSelfDefenseModule.ShockFunctionality>();
            //__instance.gameObject.TryGetComponent<PrawnSelfDefenseModule.ShockFunctionality>(out PrawnSelfDefenseModule.ShockFunctionality defenseMonobehaviour);
            //defenseMonobehaviour.gameObject.SetActive(true);
            Subtitles.Add("<color=yellow>PRAWN:</color> Self defense module for PRAWN loaded and ready.");
        }
    }
}
