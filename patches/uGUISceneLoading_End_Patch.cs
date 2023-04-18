using SMLHelper.V2.Handlers;
using Story;

namespace VELDsAlterraWeaponry.patches;

[HarmonyPatch(typeof(uGUI_SceneLoading))]
public class uGUISceneLoading_End_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(uGUI_SceneLoading.End))]
    public static void End()
    {
        AlterraWeaponry.logger.LogInfo("START EVENT - Should play an audio right now !!");
        AlterraWeaponry.AWPresentationGoal.Trigger();
        AlterraWeaponry.logger.LogInfo("Presentation audio has played.");
    }
}
