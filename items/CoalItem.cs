using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Reflection;

using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;
using UnityEngine;
using UnityEngine.Sprites;

namespace VELDsAlterraWeaponry
{
    internal class CoalItem : Craftable
    {
        public static GameObject prefab;
        public static TechType ThisTechType { get; private set; } = 0;
        public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");

        public CoalItem() : base("Coal", "Coal", "Tooltip_Coal")
        {
            OnFinishedPatching += () =>
            {
                ThisTechType = TechType;
            };
        }

        public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
        public override string[] StepsToFabricatorTab => new string[] { "Resources", "BasicMaterials" };
        public override TechGroup GroupForPDA => TechGroup.Resources;
        public override TechCategory CategoryForPDA => TechCategory.BasicMaterials;
        public override TechType RequiredForUnlock => TechType.CreepvinePiece;
        public override float CraftingTime => 7.5f;
        //public override PDAEncyclopedia.EntryData EncyclopediaEntryData => ;
        protected override Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromFile(Path.Combine(AssetsFolder, "CoalItem.png"));
        }
        protected override RecipeData GetBlueprintRecipe()
        {
            return new RecipeData()
            {
                craftAmount = 4,
                Ingredients = new List<Ingredient>(new Ingredient[]
                {
                    new Ingredient(TechType.CreepvinePiece, 1)
                })
            };
        }
        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            if (prefab == null)
            {
                CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.Titanium);
                yield return task;

                prefab = GameObject.Instantiate(task.GetResult());
            }

            GameObject go = GameObject.Instantiate(prefab);
            gameObject.Set(go);
        }
    }
}
