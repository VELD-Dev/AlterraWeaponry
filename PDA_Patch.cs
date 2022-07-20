using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

using SMLHelper.V2.Assets;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UnityEngine;

namespace VELDsAlterraWeaponry
{
    internal class PDA_Patch
    {
        //public static Dictionary<string, string> PDAEntryDesc = new Dictionary<string, string>();

        /// <summary>
        /// <c>AddPDAEntry</c> adds a PDA encyclopedia entry, handling desc, comments and languages (?)
        /// </summary>
        /// <param name="key">Key is the identifier of the created PDA Encyclopedia entry</param>
        /// <param name="name">Rendered name of the entry in the PDA (does not handle languages yet)</param>
        /// <param name="desc">Rendered paragraph under the name of the entry.</param>
        /// <param name="path">Path to the folder containing the entry</param>
        /// <param name="popup">Popup that appears when the technology is unlocked, or when it have been researched, etc...</param>
        /// <param name="image">Image rendered at the top of the Ency entry, over the title. Generally a "plan" of the object.</param>
        /// <param name="sound">Soundtrack/voiceline at the top of the Ency entry, often into Journal entries.</param>
        /// <param name="kind">Kind of the entry (Journal, Encyclopedia, TimeCapsule)</param>
        /// <param name="hidden">Wether it is hidden or not, even unlocked (maybe ?)</param>
        /// <param name="unlocked">Wether it is or not unlocked at the beginning of the game (probably).</param>
        public static void AddPDAEntry(string key, string name, string desc, string path, Sprite popup, Texture2D image, FMODAsset sound, PDAEncyclopedia.EntryData.Kind kind, bool hidden, bool unlocked)
        {
            string[] nodes = path.Split(new char[] { '/' });
            PDAEncyclopediaHandler.AddCustomEntry(new PDAEncyclopedia.EntryData
            {
                key = key,
                nodes = nodes,
                sound = sound ? sound : null,
                kind = kind,
                hidden = hidden ? hidden : false,
                unlocked = unlocked ? unlocked : false,
                image = image ? image : null,
                audio = sound ? sound : null,
                popup = popup ? popup : null
            });
            LanguageHandler.SetLanguageLine("Ency_" + key, name);
            LanguageHandler.SetLanguageLine("EncyDesc_" + key, desc);
        }
    }
}
