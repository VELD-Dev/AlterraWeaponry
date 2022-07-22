using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Logger = QModManager.Utility.Logger;
using UWE;

namespace VELDsAlterraWeaponry
{
    internal class ExplosiveTorpedoItem : Craftable
    {
        protected static GameObject prefab;
        public static TechType ThisTechType { get; private set; } = 0;

        public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");

        public ExplosiveTorpedoItem() : base("ExplosiveTorpedoItem", "ExplosiveTorpedoItem", "Tooltip_ExplosiveTorpedoItem")
        {
            OnFinishedPatching += () =>
            {
                ThisTechType = TechType;
            };
        }

        public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
        public override string[] StepsToFabricatorTab => new string[] { "Upgrades", "ExosuitUpgrades" };
        public override TechGroup GroupForPDA => TechGroup.VehicleUpgrades;
        public override TechCategory CategoryForPDA => TechCategory.VehicleUpgrades;
        public override List<TechType> CompoundTechsForUnlock => new List<TechType> { TechType.Crash, TechType.Sulphur | BlackPowderItem.ThisTechType };
        public override float CraftingTime => 5f;
        protected override Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromFile(Path.Combine(AssetsFolder, "ExplosiveTorpedo.png"));
        }
        protected override RecipeData GetBlueprintRecipe()
        {
            return new RecipeData()
            {
                craftAmount = 2,
                Ingredients = new List<Ingredient>(new Ingredient[]
                {
                    new Ingredient(TechType.Titanium, 1),
                    new Ingredient(BlackPowderItem.ThisTechType, 2)
                })
            };
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            if(prefab == null)
            {
                CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.WhirlpoolTorpedo);
                yield return gameObject;

                prefab = GameObject.Instantiate(task.GetResult());
            }

            GameObject go = GameObject.Instantiate(prefab);
            gameObject.Set(go);
        }
    }
}
