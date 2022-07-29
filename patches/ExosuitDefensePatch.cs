using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;
using UnityEngine;
using UWE;
using Logger = QModManager.Utility.Logger;


namespace VELDsAlterraWeaponry.patches
{
    internal class ExosuitDefensePatch
    {
        private static GameObject seaTruckPerimeterDefensePrefab = null;
        public static GameObject DefensePerimeterPrefab => seaTruckPerimeterDefensePrefab ??
            (seaTruckPerimeterDefensePrefab = CraftData.GetPrefabForTechTypeAsync(TechType.SeaTruckUpgradePerimeterDefense))
    }
}
