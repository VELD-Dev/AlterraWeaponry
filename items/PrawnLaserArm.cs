using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VELDsAlterraWeaponry
{
    internal class PrawnLaserArm : Equipable
    {
        public static TechType techType;
        //public static TechTag techTag;
        public string AssetFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");
        public PrawnLaserArm() : base("PrawnLaserArm", "P.R.A.W.N. Laser arm", "XenoWorx® & Alterra® collaboration technology. Extremely dangerous module for the P.R.A.W.N. Exosuit. Be infinitely cautionous with this tool. Originally made to extract minerals, it is the \"weapon\" used during the Obraxis Prime massacre, but upgraded with the Fabricators technologies.")
        {
            AlterraWeaponry.logger.LogInfo("Instantiating PrawnLaserArm...");
            OnFinishedPatching += () =>
            {
                techType = TechType;
                AlterraWeaponry.logger.LogInfo("Instantiated PrawnLaserArm.");
            };
        }

        protected static GameObject prefab;

        public override TechCategory CategoryForPDA => TechCategory.VehicleUpgrades;
        public override TechType RequiredForUnlock => TechType.PrecursorSanctuaryCube;
        public override float CraftingTime => 15f;
        public override EquipmentType EquipmentType => EquipmentType.ExosuitArm;
        public override string[] StepsToFabricatorTab => new string[] { "Upgrades", "ExosuitUpgrades" };
        public static void AddPDAEntry()
        {
            AlterraWeaponry.logger.LogInfo($"Added PDA Entry for PrawnLaserArm.");
            PDAHelper.AddEncyEntry("PrawnLaserArm", "Tech/Weaponry");
        }
        public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
        public override TechGroup GroupForPDA => TechGroup.VehicleUpgrades;
        public override QuickSlotType QuickSlotType => QuickSlotType.Instant;
        protected override RecipeData GetBlueprintRecipe()
        {
            return new RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[]
                {
                    new Ingredient(AdvancedRefractor.ThisTechType, 1),
                    new Ingredient(TechType.PlasteelIngot, 2),
                    new Ingredient(TechType.CopperWire, 4),
                    new Ingredient(TechType.AdvancedWiringKit, 2),
                    new Ingredient(TechType.AluminumOxide, 2),
                    new Ingredient(TechType.Magnetite, 2),
                    new Ingredient(TechType.Kyanite, 10),
                    new Ingredient(TechType.Diamond, 2),
                    new Ingredient(TechType.ExosuitTorpedoArmModule, 1),
                    new Ingredient(TechType.ExosuitDrillArmModule, 1),
                    new Ingredient(TechType.PrecursorIonCrystal, 2),
                })
            };
        }

        protected override Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromFile(Path.Combine(AssetFolder, "PrawnPerimeterDefense.png"));
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            if (prefab == null)
            {
                CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.ExosuitTorpedoArmModule);
                yield return task;

                prefab = GameObject.Instantiate(task.GetResult());
            }

            GameObject go = GameObject.Instantiate(prefab);
            gameObject.Set(go);

            go.AddComponent<OnPickup>();
        }
    }
}
