using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;
using UnityEngine;

namespace VELDsAlterraWeaponry
{
    internal class AdvancedRefractor : Craftable
    {
        public static GameObject prefab;
        public static TechType ThisTechType { get; private set; } = 0;
        public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");

        public AdvancedRefractor() : base("AdvancedRefractor", "AdvancedRefractor", "Tooltip_AdvancedRefractor")
        {
            OnFinishedPatching += () =>
            {
                ThisTechType = TechType;
            };
        }

        public override TechCategory CategoryForPDA => TechCategory.AdvancedMaterials;
        public override TechGroup GroupForPDA => TechGroup.Resources;
        public override float CraftingTime => 3f;
        public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
        public override string[] StepsToFabricatorTab => new string[] { "Resources", "Electronics" };
        public override TechType RequiredForUnlock => TechType.AramidFibers;
        protected override Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromFile(Path.Combine(AssetsFolder, "PrawnPerimeterDefense.png"));
        }
        protected override RecipeData GetBlueprintRecipe()
        {
            return new RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[]
                {
                    new Ingredient(CoalItem.ThisTechType, 1),
                    new Ingredient(TechType.Silver, 4),
                    new Ingredient(TechType.ComputerChip, 1),
                    new Ingredient(TechType.AramidFibers, 2),
                    new Ingredient(TechType.Benzene, 1),
                    new Ingredient(TechType.EnameledGlass, 4)
                })
            };
        }
        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            if (prefab == null)
            {
                CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.PDA);
                yield return task;

                prefab = GameObject.Instantiate(task.GetResult());
            }

            GameObject go = GameObject.Instantiate(prefab);
            gameObject.Set(go);
        }
    }
}
