namespace VELDsAlterraWeaponry.patches;

[HarmonyPatch(typeof(uGUI_SceneLoading))]
public class uGUISceneLoading_End_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(uGUI_SceneLoading.End))]
    public static void EndPatch()
    {
        AlterraWeaponry.logger.LogInfo("START EVENT - Should play an audio right now !!");
        //PDASounds.queue.Play("presentation", SoundHost.PDA, true, subtitles: "Subtitles_AWPresentation");
        /*FMODAsset sound = new()
        {
            name = "AWPresentation",
            id = "AWPresentation",
            path = Path.Combine(Assembly.GetExecutingAssembly().Location, "/assets/first_lethal_weapon_message.ogg"),
        };
        PDAHelper.AddLogEntry(key: "presentation", languageKey: "Subtitles_AWPresentation", sound: sound);*/
        PDAHelper.PlayPDAVoiceLineFMOD
        (
            Path.Combine(AlterraWeaponry.AssetFolder, "first_lethal_weapon_message.mp3"),
            "Subtitles_AWPresentation"
        );
        AlterraWeaponry.logger.LogInfo("Presentation audio has played.");
    }
}
