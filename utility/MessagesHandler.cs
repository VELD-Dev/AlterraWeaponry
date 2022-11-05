using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using SMLHelper.V2.FMod.Interfaces;
using Logger = QModManager.Utility.Logger;
using SMLHelper.V2.FMod;

namespace VELDsAlterraWeaponry
{
    internal class MessagesHandler
    {
        /// <summary>
        /// Register a PDA Message entry
        /// </summary>
        /// <param name="soundId">Identifier through the game of the sound.</param>
        /// <param name="path">Path to the sound file (ABSOLUTE).</param>
        public static void AddMessageEntry(string soundId, string path)
        {
            Logger.Log(Logger.Level.Info, $"Adding message entry {soundId}...");
            Subtitles.Add(soundId, new string[] { soundId });
            CustomSoundHandler.RegisterCustomSound(soundId, path, AudioUtils.BusPaths.PDAVoice);
            //StoryGoalCustomEventHandler.main.SendMessage(methodName: soundId, options: UnityEngine.SendMessageOptions.DontRequireReceiver, value: path);
            Logger.Log(Logger.Level.Info, $"Added {soundId} message entry successfully");
        }
    }
}
