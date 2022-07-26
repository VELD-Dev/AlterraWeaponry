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

    }
}
