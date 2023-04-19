using VELDsAlterraWeaponry;

namespace SubnauticaHelper;

public class MessagesHandler
{
    internal static VoiceNotificationManager voiceNotifManager = new();
    internal static VoiceNotification voiceNotif = new();
    /// <summary>
    /// Register a PDA Message entry
    /// </summary>
    /// <param name="soundId">Identifier through the game of the sound.</param>
    /// <param name="path">Path to the sound file (ABSOLUTE).</param>
    public static void AddMessageEntry(string soundId, string path)
    {
        AlterraWeaponry.logger.LogInfo($"Adding message entry {soundId}...");
        Subtitles.Add(soundId, new string[] { soundId });
        CustomSoundHandler.RegisterCustomSound(soundId, path, AudioUtils.BusPaths.PDAVoice);
        //StoryGoalCustomEventHandler.main.SendMessage(methodName: soundId, options: UnityEngine.SendMessageOptions.DontRequireReceiver, value: path);
        AlterraWeaponry.logger.LogInfo($"Added {soundId} message entry successfully");
    }

    /// <summary>
    /// Plays a PDA message directly from sound file.
    /// </summary>
    /// <param name="soundId">Identifier through the game of the sound. Is also used as subtitle ID.</param>
    /// <param name="path">Path to the sound file (ABSOLUTE).</param>
    public static void PlayNewMessage(string soundId, string path)
    {
        AlterraWeaponry.logger.LogInfo($"Trying to play {soundId}");
        Subtitles.Add(soundId, new[] { soundId });
        PDALog.EntryData PDALogEntry = PDALog.Add(soundId, true);
        PDALogEntry.sound = new()
        {
            path = path,
            id = soundId,
            name = soundId,
        };
    }
}
