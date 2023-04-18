using Story;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VELDsAlterraWeaponry.patches;

[HarmonyPatch(typeof(ItemGoalTracker))]
public class ItemGoalTracker_Start_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch("Start")]
    public static void Start(ItemGoalTracker __instance)
    {
        List<TechType> techTypes = new List<TechType>()
        {
            ExplosiveTorpedoItem.ThisTechType,
            PrawnLaserArm.techType,
        };

        var goals = __instance.goalData.goals;
        foreach (var techType in techTypes)
        {
            var goal = new ItemGoal()
            {
                techType = ExplosiveTorpedoItem.ThisTechType,
                goalType = Story.GoalType.PDA,
                key = $"Goal_FirstLethal"
            };

            goals = goals.AddItem(goal).ToArray();
        }
        __instance.goalData.goals = goals;
    }
}
