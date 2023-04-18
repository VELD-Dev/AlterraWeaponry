namespace VELDsAlterraWeaponry.patches;

[HarmonyPatch(typeof(GameSettings))]
public class GameSettingsPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameSettings.SaveAsync))]
    public static void Postfix()
    {
        LanguagesHandler.LanguagePatch();
    }
}
