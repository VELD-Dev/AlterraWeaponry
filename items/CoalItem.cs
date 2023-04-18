using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VELDsAlterraWeaponry
{
    internal class CoalItem : Craftable
    {
        public static GameObject prefab;
        public static TechType ThisTechType { get; private set; } = 0;
        public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");

        public CoalItem() : base("Coal", "Coal", "Tooltip_Coal")
        {
            AlterraWeaponry.logger.LogInfo("Instantiating CoalItem...");
            OnFinishedPatching += () =>
            {
                ThisTechType = TechType;
                AlterraWeaponry.logger.LogInfo("Instantiated CoalItem.");
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
            go.AddComponent<OnPickup>();
            gameObject.Set(go);
        }
    }
}
