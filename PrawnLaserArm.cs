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
using UnityEngine;
using System.Collections;

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
        public override List<TechType> CompoundTechsForUnlock => new List<TechType> { TechType.ExosuitDrillArmModule, TechType.ExosuitTorpedoArmModule, TechType.ExosuitGrapplingArmModule };
        public override float CraftingTime => 60f;
        public override string DiscoverMessage => "This tool is the result of a collaboration between XenoWorx® and Alterra®. Originally imaginated to extract ores, it have been misused and used as a weapon by a crazy miner on the planet Obraxis Prime. After this event, Alterra® and XenoWorx® have totally stopped working together. To avoid a drama, Alterra® left this blueprint under XenoWorx® possession.";
        public override EquipmentType EquipmentType => EquipmentType.ExosuitArm;
        public override PDAEncyclopedia.EntryData EncyclopediaEntryData => new PDAEncyclopedia.EntryData()
        {
            key = "test1",
            path = "test1",
            audio = new FMODAsset(),
            kind = PDAEncyclopedia.EntryData.Kind.Encyclopedia,
            popup = ImageUtils.LoadSpriteFromFile(Path.Combine(AssetFolder, "ExplosiveTorpedo.png")),
            image = Texture2D.blackTexture,
            hidden = false,
            unlocked = false,
            nodes = new string[] { "test" }
        };
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
                }) // DO NOT FORGET TO ADD THE FUCKING ADVANCED REFLECTOR Lu86K
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
        }
    }
}
