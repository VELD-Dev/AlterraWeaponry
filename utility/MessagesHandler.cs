using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using Logger = QModManager.Utility.Logger;

namespace VELDsAlterraWeaponry
{
    internal class MessagesHandler
    {
        public static void AddMessageEntry(string soundId, string path)
        {
            Logger.Log(Logger.Level.Info, $"Adding message entry {soundId}...");
            CustomSoundHandler.RegisterCustomSound(soundId, path, AudioUtils.BusPaths.PDAVoice);
            Logger.Log(Logger.Level.Info, $"Added {soundId} message entry successfully");
        }
    }
}
