﻿using UnityEngine;

namespace VELDsAlterraWeaponry;

[Menu("Alterra® Weaponry™")]
public class ModConfigs : SMLHelper.V2.Json.ConfigFile
{
    [Toggle(LabelLanguageId = "Options.AW_DialogsBool", TooltipLanguageId = "Options.AW_DialogsBool.Tooltip")]
    public bool DialogsEnabled = true;

    [Slider(LabelLanguageId = "Options.AW_dmgMultiplier", TooltipLanguageId = "Options.AW_dmgMultiplier.Tooltip", Min = 0.25f, Max = 5.0f, DefaultValue = 1.0f, Format = "x{0:F2}")]
    public float dmgMultiplier = 1.0f;

    [Keybind(LabelLanguageId = "Options.AW_KeybindPrawnSpecial", TooltipLanguageId = "Options.AW_KeybindPrawnSpecial.Tooltip")]
    public KeyCode specialKey = KeyCode.V;
}
