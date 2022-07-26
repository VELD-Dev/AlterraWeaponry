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
    [HarmonyPatch(typeof(Vehicle))]
    internal class ExosuitPatches
    {
        [HarmonyPatch("OnUpgradeModuleUse")]
        public static void Postfix(TechType techType)
        {
            if(techType == PrawnSelfDefenseModule.thisTechType)
            {
                var edEffect = new ElectricalDefense();
                Logger.Log(Logger.Level.Debug, "Input received", showOnScreen: true);
                edEffect.Invoke("ElectricalDefense", 1f);
            }
        }
    }
}
