using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;

namespace VELDsAlterraWeaponry
{
    [Menu("Alterra© Weaponry™")]
    public class ModConfigs : ConfigFile
    {
        [Toggle(/*LabelLanguageId = "Options.DialogsBool",*/ Label = "Allow Dialogs", Tooltip = "Whether or not dialog boxes about the lore of the Alterra Weaponry will appear at the first lethal weapon have been build.")]
        public bool DialogsEnabled = true;

        [Slider(/*LabelLanguageId = "Options.dmgMultiplier",*/ Label = "Damage multiplier", Tooltip = "The damage multiplier of the lethal weapons.", Min = 0.25f, Max = 5.0f, DefaultValue = 1.0f, Format = "{0:F2}")]
        public float dmgMultiplier = 1.0f;

        [Keybind(/*LabelLanguageId = "Options.KeybindPrawnSpecial",*/ "Prawn Special", Tooltip = "Specials can also be called 'Modules Abilities', for example a self-defense system (like the Seatruck one).")]
        public KeyCode specialKey = KeyCode.V;
    }
}
