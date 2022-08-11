using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;
using UnityEngine;

namespace VELDsAlterraWeaponry
{
    internal class PrawnSelfDefenseModule : Equipable
    {
        public static TechType thisTechType;

        public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");

        public PrawnSelfDefenseModule() : base("PrawnSelfDefenseModule", "PrawnDefensePerimeter", "Tooltip_PrawnDefensePerimeter")
        {
            OnFinishedPatching += () =>
            {
                thisTechType = TechType;
            };
        }

        protected static GameObject prefab;

        public override EquipmentType EquipmentType => EquipmentType.ExosuitModule;
        public override TechType RequiredForUnlock => TechType.SeaTruckUpgradePerimeterDefense;
        public override TechGroup GroupForPDA => TechGroup.VehicleUpgrades;
        public override TechCategory CategoryForPDA => TechCategory.VehicleUpgrades;
        public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
        public override string[] StepsToFabricatorTab => new string[] { "Upgrades", "ExosuitUpgrades" };
        public override float CraftingTime => 3f;
        public override QuickSlotType QuickSlotType => QuickSlotType.Instant;
        protected override Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromFile(Path.Combine(AssetsFolder, "PrawnPerimeterDefense.png"));
        }
        public static void AddPDAEntry()
        {
            PDA_Patch.AddPDAEntry("PrawnDefensePerimeter", "Weaponry");
        }
        protected override RecipeData GetBlueprintRecipe()
        {
            return new RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient> (new Ingredient[]
                {
                    new Ingredient (TechType.AluminumOxide, 4),
                    new Ingredient (TechType.Battery, 2),
                    new Ingredient (TechType.AdvancedWiringKit, 1),
                    new Ingredient (TechType.SeaTruckUpgradePerimeterDefense, 1)
                })
            };
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            if(prefab == null)
            {
                CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.SeaTruckUpgradePerimeterDefense);
                yield return task;

                prefab = GameObject.Instantiate(task.GetResult());
            }

            GameObject go = GameObject.Instantiate(prefab);
            gameObject.Set(go);
        }

        internal class ExosuitDefenseMono : MonoBehaviour
        {
            private static GameObject seaTruckPerimeterDefensePrefab = null;
            public static GameObject DefensePerimeterPrefab => seaTruckPerimeterDefensePrefab ??
                (seaTruckPerimeterDefensePrefab = CraftData.GetPrefabForTechTypeAsync(TechType.SeaTruckUpgradePerimeterDefense).GetResult());

            private const float EnergyCostPerUse = 1f;
            private const float Power = 2.5f;
            private const float DamageMultiplier = 1f;
            private const float BaseCharge = 2f;
            private const float BaseRadius = 1f;

            private static float DirectDefDamage = (BaseRadius + Power * BaseCharge) * DamageMultiplier * 0.5f;

            public static float DefenseCooldown = 5f;
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

            public bool Use(Vehicle exosuit, GameObject obj)
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

                if (GameModeUtils.RequiresPower())
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
}
