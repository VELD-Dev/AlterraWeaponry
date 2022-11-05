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
using Logger = QModManager.Utility.Logger;

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
            PDA_Patch.AddPDAEntry("PrawnDefensePerimeter", "Tech/Weaponry");
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



        /*

        SHOCK FUNCTIONALITY

         */

        internal class ShockFunctionality : MonoBehaviour // Thanks to EldritchCarMaker, and indirectly to PrimeSonic 👍
        {
            private IEnumerator<GameObject> SetupSeatruckPrefab()
            {
                var seatruckElectricalDefensePF = CraftData.GetPrefabForTechTypeAsync(TechType.SeaTruck).GetResult().GetComponent<SeaTruckUpgrades>().electricalDefensePrefab;
                yield return seatruckElectricalDefensePF;
            }
            private void Start()
            {
                StartCoroutine(nameof(SetupSeatruckPrefab));
            }
            private GameObject seamothElectricalDefensePrefab = null;
            public GameObject ElectricalDefensePrefab => seamothElectricalDefensePrefab ??
                (seamothElectricalDefensePrefab = SetupSeatruckPrefab().Current);

            private const float EnergyCostPerShock = 5;
            private const float ShockPower = 6f;
            private const float BaseCharge = 2f;
            private const float BaseRadius = 1f;

            public const float ShockCooldown = 10f;
            public float timeNextZap = 0;
            private static float DamageMultiplier => AlterraWeaponry.Config.dmgMultiplier;
            private static float DirectZapDamage = (BaseRadius + ShockPower * BaseCharge) * DamageMultiplier * 0.5f;
            // Calculations and initial values based off ElectricalDefense component

            public static bool AbleToShock(Vehicle vehicle)
            {
                if (GameModeManager.GetOption<bool>(GameOption.TechnologyRequiresPower) && vehicle.GetComponent<EnergyMixin>().charge < EnergyCostPerShock)
                    return false;

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

            public bool Use(Vehicle vehicle, GameObject obj)
            {
                Logger.Log(Logger.Level.Debug, "Trying to use...", showOnScreen: true);
                if (Time.time < timeNextZap)
                    return true;

                Logger.Log(Logger.Level.Debug, "Timeout: OK", showOnScreen: true);
                if (!AbleToShock(vehicle))
                    return false;
                Logger.Log(Logger.Level.Debug, "Able to shock: OK", showOnScreen: true);

                ShockRadius(vehicle);
                Logger.Log(Logger.Level.Debug, "Shocked nothing.", showOnScreen: true);

                if (!IsTargetValid(obj))
                    return true;
                Logger.Log(Logger.Level.Debug, "Valid target(s): OK", showOnScreen: true);

                ZapCreature(vehicle, obj);
                timeNextZap = Time.time + ShockCooldown;
                Logger.Log(Logger.Level.Debug, "Shocked !", showOnScreen: true );
                return true;
            }

            private void ShockRadius(Vehicle originVehicle)
            {
                if (originVehicle == null)
                    return;

                GameObject gameObject = Utils.SpawnZeroedAt(ElectricalDefensePrefab, originVehicle.transform, false);
                ElectricalDefense defenseComponent = gameObject.GetComponent<ElectricalDefense>();
                defenseComponent.charge = ShockPower;
                defenseComponent.chargeScalar = ShockPower;
                defenseComponent.radius *= ShockPower;
                defenseComponent.chargeRadius *= ShockPower;

                if (GameModeManager.GetOption<bool>(GameOption.TechnologyRequiresPower))
                    originVehicle.GetComponent<EnergyMixin>().ConsumeEnergy(EnergyCostPerShock);
            }

            private static void ZapCreature(Vehicle originVehicle, GameObject target)
            {
                if (originVehicle == null || target == null)
                    return;

                target.GetComponent<LiveMixin>().TakeDamage(DirectZapDamage, default, DamageType.Electrical, originVehicle.gameObject);

                if (GameModeManager.GetOption<bool>(GameOption.TechnologyRequiresPower))
                    originVehicle.GetComponent<EnergyMixin>().ConsumeEnergy(EnergyCostPerShock);
            }
        }
    }
}
