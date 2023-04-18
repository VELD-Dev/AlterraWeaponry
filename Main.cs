using Story;
using UnityEngine;

namespace VELDsAlterraWeaponry;


[BepInPlugin(modGUID, modName, version)]
public class AlterraWeaponry : BaseUnityPlugin
{
    internal static ModConfigs Configs { get; set; } // = OptionsPanelHandler.RegisterModOptions<ModConfigs>();
    private const string modGUID = "com.velddev.alterraweaponry";
    private const string modName = "Alterra Weaponry";
    private const string version = "0.0.3";

    private static readonly Harmony harmony = new(modGUID);

    public static ManualLogSource logger;
    internal static string AssetFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");

    // STORY GOALS
    internal static StoryGoal AWPresentationGoal = new("Goal_AWPresentation", Story.GoalType.PDA, 8f);

    private void Awake()
    {
        logger = Logger;
        logger.LogInfo($"{modName} started patching.");
        harmony.PatchAll();
        logger.LogInfo($"{modName} patched harmony patches. Now adding items.");

        // PDA stuff
        PrawnLaserArm.AddPDAEntry();
        PrawnSelfDefenseModule.AddPDAEntry();
        PDAHelper.AddEncyEntry("AWModInfo", "Meta", hidden: false, unlocked: true);
        // Messages
        CustomSoundHandler.RegisterCustomSound("Goal_FirstLethal", Path.Combine(AssetFolder, "first_lethal_weapon_message.ogg"), AudioUtils.BusPaths.PDAVoice);
        FMODAsset firstLethal = ScriptableObject.CreateInstance<FMODAsset>();
        firstLethal.path = "Goal_FirstLethal";
        PDALogHandler.AddCustomEntry(
            "Goal_FirstLethal", 
            "Subtitles_AWFirstLethal", 
            sound: firstLethal
        );
        CustomSoundHandler.RegisterCustomSound("Goal_AWPresentation", Path.Combine(AssetFolder, "xenoworx_pda_presentation.mp3"), AudioUtils.BusPaths.PDAVoice);
        FMODAsset presentationMessage = ScriptableObject.CreateInstance<FMODAsset>();
        presentationMessage.path = "Goal_AWPresentation";
        PDALogHandler.AddCustomEntry(
            "Goal_AWPresentation",
            "Subtitles_AWPresentation",
            sound: presentationMessage
        );
        logger.LogInfo("Added PDA Ency entry 'AWModInfo' (meta)");
        //MessagesHandler.AddMessageEntry("presentation", Path.Combine(AssetFolder, "xenoworx_pda_presentation.mp3"));
        //MessagesHandler.AddMessageEntry("first_lethal", Path.Combine(AssetFolder, "first_lethal_weapon_message.mp3"));

        ExplosiveTorpedoItem explosiveTorpedo = new();
        PrawnSelfDefenseModule prawnDefenseModule = new();
        PrawnLaserArm prawnLaserArm = new();

        new CoalItem().Patch();
        new BlackPowderItem().Patch();
        new AdvancedRefractor().Patch();
        explosiveTorpedo.Patch();
        prawnDefenseModule.Patch();
        prawnLaserArm.Patch();

        logger.LogInfo("Added all items. Patching localizations...");

        LanguagesHandler.LanguagePatch();
        logger.LogInfo(modName + " " + version + " loaded.");

        /* DO NOT PAY ATTENTION. THIS IS TEMPORARILY UNAVAILABLE.
        AlterraWeaponry.logger.LogInfo("Trying patching option menu...");
        AlterraWeaponry.Configs = OptionsPanelHandler.RegisterModOptions<ModConfigs>();
        AlterraWeaponry.logger.LogInfo("Loaded config file.");
        */
    }
}
