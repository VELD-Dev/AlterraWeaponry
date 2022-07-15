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

namespace VELDsAlterraWeaponry
{
    internal class ExplosiveTorpedoItem : Craftable
    {
        public static TechType thisTechType;

        public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");

        public ExplosiveTorpedoItem() : base("ExplosiveTorpedoItem", "Explosive Torpedo", "A new lethal weapon designed by Alterra© for missions on 4546B after the result of previous Alterra© missions.")
        {
            OnFinishedPatching += () =>
            {
                thisTechType = TechType;
            };
        }

        protected static GameObject prefab;

        public override CraftTree.Type FabricatorType => CraftTree.Type.SeamothUpgrades;
        public override string[] StepsToFabricatorTab => new string[] { "SeaTruckUpgrade" };
        public override TechGroup GroupForPDA => TechGroup.VehicleUpgrades;
        public override TechCategory CategoryForPDA => TechCategory.VehicleUpgrades;
        public override string DiscoverMessage => "Alterra© has made an exception for the expeditions on planet 4546B, and has allowed some lethal weapons. Be extremely careful, these are dangerous.";
        public override List<TechType> CompoundTechsForUnlock => new List<TechType> { TechType.Crash, TechType.Sulphur };
        public override float CraftingTime => 5f;
        //public override PDAEncyclopedia.EntryData EncyclopediaEntryData => ;
        //public override QuickSlotType QuickSlotType => QuickSlotType.Passive;
        protected override Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromFile(Path.Combine(AssetsFolder, "ExplosiveTorpedo.png"));
        }
        protected override RecipeData GetBlueprintRecipe()
        {
            return new RecipeData()
            {
                craftAmount = 4,
                Ingredients = new List<Ingredient>(new Ingredient[]
                {
                    new Ingredient(TechType.Titanium, 1),
                    new Ingredient(TechType.Sulphur, 1),
                    new Ingredient(TechType.GasTorpedo, 2)
                })
            };
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            if(prefab == null)
            {
                CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.GasTorpedo);
                yield return gameObject;

                prefab = GameObject.Instantiate(task.GetResult());
            }

            GameObject go = GameObject.Instantiate(prefab);
            gameObject.Set(go);
        }
    }
}
