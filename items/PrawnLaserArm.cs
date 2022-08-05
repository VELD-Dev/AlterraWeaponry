using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;
using SMLHelper.V2.Handlers;
using UnityEngine;
using System.Collections;
using HarmonyLib;

namespace VELDsAlterraWeaponry
{
    internal class PrawnLaserArm : Equipable
    {
        public static TechType techType;
        //public static TechTag techTag;
        public string AssetFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");
        public PrawnLaserArm() : base("PrawnLaserArm", "P.R.A.W.N. Laser arm", "XenoWorx® & Alterra® collaboration technology. Extremely dangerous module for the P.R.A.W.N. Exosuit, be infinitely careful with this tool. Originally made to extract minerals, it was the \"weapon\" used during the Obraxis Prime massacre, but upgraded with the Fabricators technologies.")
        {
            OnFinishedPatching += () =>
            {
                techType = TechType;
            };
        }

        protected static GameObject prefab;

        public override TechCategory CategoryForPDA => TechCategory.VehicleUpgrades;
        public override TechType RequiredForUnlock => TechType.PrecursorSanctuaryCube;
        public override float CraftingTime => 60f;
        public override EquipmentType EquipmentType => EquipmentType.ExosuitArm;
        public override string[] StepsToFabricatorTab => new string[] { "Upgrades", "ExosuitUpgrades" };
        public static void AddPDAEntry()
        {
            PDA_Patch.AddPDAEntry("PrawnLaserArm", "Weaponry");
        }
        public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
        public override TechGroup GroupForPDA => TechGroup.VehicleUpgrades;
        public override QuickSlotType QuickSlotType => QuickSlotType.Instant;
        protected override RecipeData GetBlueprintRecipe()
        {
            return new RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient> (new Ingredient[]
                {
                    new Ingredient(AdvancedRefractor.ThisTechType, 1),
                    new Ingredient(TechType.PlasteelIngot, 2),
                    new Ingredient(TechType.Quartz, 4),
                    new Ingredient(TechType.Copper, 4),
                    new Ingredient(TechType.AdvancedWiringKit, 2),
                    new Ingredient(TechType.AluminumOxide, 5),
                    new Ingredient(TechType.Magnetite, 5),
                    new Ingredient(TechType.ComputerChip, 2),
                    new Ingredient(TechType.Kyanite, 5),
                    new Ingredient(TechType.Diamond, 5),
                    new Ingredient(TechType.ExosuitTorpedoArmModule, 1),
                    new Ingredient(TechType.ExosuitDrillArmModule, 1),
                    new Ingredient(TechType.ExosuitGrapplingArmModule, 1),
                    new Ingredient(TechType.PrecursorIonCrystal, 5),
                    new Ingredient(TechType.PrecursorIonPowerCell, 2)
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
