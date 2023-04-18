using System.Collections.Generic;
using UnityEngine;

namespace VELDsAlterraWeaponry.behaviours;
public class OnPickup : MonoBehaviour
{
    public static List<TechType> techTypes = new() {
            new ExplosiveTorpedoItem().TechType,
            new PrawnLaserArm().TechType
        };

    public void Awake()
    {
    }

    public void OnExamine()
    {
        AlterraWeaponry.logger.LogInfo("Should try to see if a picked-up item is registered in techTypes");
        foreach (var techType in techTypes)
        {
            if (KnownTech.Contains(techType)) return;
        }
        AlterraWeaponry.logger.LogInfo("Yes, it should play an audio right now !!");
        PDASounds.queue.Play("first_lethal", SoundHost.PDA, true, subtitles: "Subtitles_AWFirstLethal");
        PDAEncyclopedia.Reveal("ExplosiveTorpedo", false);
    }
}
