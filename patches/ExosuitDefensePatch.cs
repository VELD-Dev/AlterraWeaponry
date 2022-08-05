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
            (seaTruckPerimeterDefensePrefab = CraftData.GetPrefabForTechTypeAsync(TechType.SeaTruckUpgradePerimeterDefense).GetResult());

        private const float EnergyCostPerUse = 1f;
        private const float Power = 2.5f;
        private const float DamageMultiplier = 1f;
        private const float BaseCharge = 2f;
        private const float BaseRadius = 1f;

        private const float DirectDefDamage = (BaseRadius + Power * BaseCharge) * DamageMultiplier * 0.5f;

        public const float DefenseCooldown = 5f;
        public static float timeNextUse = 0;

        public static bool AbleToUse(Vehicle exosuit)
        {
            if (GameModeUtils.RequiresPower() && exosuit.GetComponent<EnergyMixin>().charge < EnergyCostPerUse) return false;

            return true;
        }
        public static bool IsTargetValid(GameObject target)
        {
            if (target == null)
                return false;
            if (!target.TryGetComponent(out LiveMixin mixin))
                return false;

            return mixin.IsAlive();
        }

        public static bool Use(Vehicle exosuit, GameObject obj)
        {
            if (Time.time < timeNextUse)
                return true;

            if (!AbleToUse(exosuit))
                return false;

            DefenseRadius(exosuit);

            if (!IsTargetValid(obj))
                return true;

            ShockCreature(exosuit, obj);
            timeNextUse = Time.time + DefenseCooldown;
            return true;

        }

        private static void DefenseRadius(Vehicle originVehicle)
        {
            if (originVehicle == null)
                return;

            GameObject gameObject = Utils.SpawnZeroedAt(DefensePerimeterPrefab, originVehicle.transform, false);
            ElectricalDefense defenseComponent = gameObject.GetComponent<ElectricalDefense>();
            defenseComponent.charge = Power;
            defenseComponent.radius *= Power;
            defenseComponent.chargeScalar = Power;
            defenseComponent.chargeRadius *= Power;

            if(GameModeUtils.RequiresPower())
                originVehicle.GetComponent<EnergyMixin>().ConsumeEnergy(EnergyCostPerUse);
        }

        private static void ShockCreature(Vehicle originVehicle, GameObject target)
        {
            if (originVehicle == null || target == null)
                return;

            target.GetComponent<LiveMixin>().TakeDamage(DirectDefDamage, default, DamageType.Electrical, originVehicle.gameObject);

            if (GameModeUtils.RequiresPower())
                originVehicle.GetComponent<EnergyMixin>().ConsumeEnergy(EnergyCostPerUse);
        }
    }
}
