using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VELDsAlterraWeaponry
{
    public class ExplosiveTorpedoItem : Craftable
    {
        protected static GameObject prefab;
        public static TechType ThisTechType { get; private set; } = 0;

        public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");

        public ExplosiveTorpedoItem() : base("ExplosiveTorpedoItem", "ExplosiveTorpedoItem", "Tooltip_ExplosiveTorpedoItem")
        {
            AlterraWeaponry.logger.LogInfo("Instantiating ExplosiveTorpedoItem...");
            OnFinishedPatching += () =>
            {
                ThisTechType = TechType;
                AlterraWeaponry.logger.LogInfo("Instantiated ExplosiveTorpedoItem.");
            };
        }

        public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
        public override string[] StepsToFabricatorTab => new string[] { "Upgrades", "ExosuitUpgrades" };
        public override TechGroup GroupForPDA => TechGroup.VehicleUpgrades;
        public override TechCategory CategoryForPDA => TechCategory.VehicleUpgrades;
        public override TechType RequiredForUnlock => BlackPowderItem.ThisTechType;
        public override float CraftingTime => 3.5f;
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
            if (prefab == null)
            {
                CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.GasTorpedo);
                yield return gameObject;

                prefab = GameObject.Instantiate(task.GetResult());
            }

            GameObject go = GameObject.Instantiate(prefab);
            gameObject.Set(go);

            go.AddComponent<OnPickup>();
        }
    }
}
