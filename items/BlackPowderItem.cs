using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;
using System.Reflection;
using UnityEngine;

namespace VELDsAlterraWeaponry
{
    internal class BlackPowderItem : Craftable
    {
        protected static GameObject prefab;

        public static TechType ThisTechType { get; private set; } = 0;

        public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");

        public BlackPowderItem() : base("BlackPowder", "BlackPowder", "Tooltip_BlackPowder")
        {
            OnFinishedPatching += () =>
            {
                ThisTechType = TechType;
            };
        }

        public override List<TechType> CompoundTechsForUnlock => new List<TechType> { TechType.Crash, TechType.Sulphur, TechType.JeweledDiskPiece, CoalItem.ThisTechType }; // DO NOT FORGET TO ADD CreepvineCoal.thisTechType
        public override TechGroup GroupForPDA => TechGroup.Resources;
        public override TechCategory CategoryForPDA => TechCategory.AdvancedMaterials;
        public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
        public override string[] StepsToFabricatorTab => new string[] { "Resources", "AdvancedMaterials" };
        public override float CraftingTime => 3f;
        protected override UnityEngine.Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromFile(Path.Combine(AssetsFolder, "CoalItem.png"));
        }
        protected override RecipeData GetBlueprintRecipe()
        {
            return new RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[]
                {
                    new Ingredient (TechType.Sulphur, 1),
                    new Ingredient (CoalItem.ThisTechType, 1),
                    new Ingredient (TechType.JeweledDiskPiece, 3)
                })
            };
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            if (prefab == null)
            {
                CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.HydrochloricAcid);
                yield return task;

                prefab = GameObject.Instantiate(task.GetResult());
            }

            GameObject go = GameObject.Instantiate(prefab);
            gameObject.Set(go);
        }
    }
}
