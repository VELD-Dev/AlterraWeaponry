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
        /// <param name="path">Path to the folder containing the entry</param>
        /// <param name="popup">Popup that appears when the technology is unlocked, or when it have been researched, etc... <br/>Default value: <c>null</c></param>
        /// <param name="image">Image rendered at the top of the Ency entry, over the title. Generally a "plan" of the object. <br/>Default value: <c>null</c></param>
        /// <param name="sound">Soundtrack/voiceline at the top of the Ency entry, often into Journal entries. <br/>Default value: <c>null</c></param>
        /// <param name="kind">Kind of the entry (Journal, Encyclopedia, TimeCapsule). <br/>Default value: <c>PDAEncyclopedia.EntryData.Kind.Encyclopedia</c></param>
        /// <param name="hidden">Wether it is hidden or not, even unlocked (maybe ?). <br/>Default value: <c>false</c></param>
        /// <param name="unlocked">Wether it is or not unlocked at the beginning of the game (probably). <br/>Default value: <c>false</c></param>
        public static void AddPDAEntry(string key, string path, Sprite popup = null, Texture2D image = null, FMODAsset sound = null, PDAEncyclopedia.EntryData.Kind kind = PDAEncyclopedia.EntryData.Kind.Encyclopedia, bool hidden = false, bool unlocked = false)
        {
            string[] nodes = path.Split(new char[] { '/' });
            PDAEncyclopediaHandler.AddCustomEntry(new PDAEncyclopedia.EntryData
            {
                key = key,
                nodes = nodes,
                sound = sound,
                kind = kind,
                hidden = hidden,
                unlocked = unlocked,
                image = image,
                audio = sound,
                popup = popup
            });
        }
    }
}
