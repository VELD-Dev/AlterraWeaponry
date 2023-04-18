using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VELDsAlterraWeaponry
{
    internal class PrawnSelfDefenseModule : Equipable
    {
        public static TechType thisTechType;

        public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");

        public PrawnSelfDefenseModule() : base("PrawnSelfDefenseModule", "PrawnDefensePerimeter", "Tooltip_PrawnDefensePerimeter")
        {
            AlterraWeaponry.logger.LogInfo("Instantiating PrawnSelfDefenseModule...");
            OnFinishedPatching += () =>
            {
                thisTechType = TechType;
                AlterraWeaponry.logger.LogInfo("Instantiated PrawnSelfDefenseModule.");
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
            AlterraWeaponry.logger.LogInfo("Added PDA entry for PrawnSelfDefenseModule.");
            PDAHelper.AddEncyEntry("PrawnDefensePerimeter", "Tech/Weaponry");
        }
        protected override RecipeData GetBlueprintRecipe()
        {
            return new()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[]
                {
                    new(TechType.AluminumOxide, 4),
                    new(TechType.AdvancedWiringKit, 1),
                    new(TechType.AramidFibers, 1)
                })
            };
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            if (prefab == null)
            {
                CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.SeaTruckUpgradePerimeterDefense);
                yield return task;

                prefab = GameObject.Instantiate(task.GetResult());
            }

            GameObject go = GameObject.Instantiate(prefab);
            go.AddComponent<OnPickup>();
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
            private static float DamageMultiplier => AlterraWeaponry.Configs.dmgMultiplier;
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
                AlterraWeaponry.logger.LogInfo("Trying to use...");
                if (Time.time < timeNextZap)
                    return true;

                AlterraWeaponry.logger.LogInfo("Timeout: OK");
                if (!AbleToShock(vehicle))
                    return false;
                AlterraWeaponry.logger.LogInfo("Able to shock: OK");

                ShockRadius(vehicle);
                AlterraWeaponry.logger.LogInfo("Shocked nothing.");

                if (!IsTargetValid(obj))
                    return true;
                AlterraWeaponry.logger.LogInfo("Valid target(s): OK");

                ZapCreature(vehicle, obj);
                timeNextZap = Time.time + ShockCooldown;
                AlterraWeaponry.logger.LogInfo("Shocked !");
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
