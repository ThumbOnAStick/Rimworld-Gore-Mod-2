using HarmonyLib;
using System;
using Verse;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityFragments
{
    [StaticConstructorOnStartup]
    internal class MetaPatches
    {
        public static Harmony harmony;

        static MetaPatches()
        {
            Harmony harmony;
            if ((harmony = MetaPatches.harmony) == null)
            {
                harmony = (MetaPatches.harmony = new Harmony("thumb.VBFragments"));
            }
            try
            {
                MetaPatches.harmony = harmony;
                DismemberPatch.PatchHarmony();
                // Remove all listeners first
                CompDeathRecorder.PawnKilledEvent?.RemoveAllListeners();
                // Add pawn death patch
                CompDeathRecorder.PawnKilledEvent?.AddListener(DismembermentUtils.TrySpawnTorsoFragment);
            }
            catch (Exception e)
            {
                Dialog_MessageBox box = new Dialog_MessageBox($"[Visual Brutalty] a severe error occured while trying to patch harmony, you can report this bug on bug report thread. {e}");
                Find.WindowStack.Add(box);
            }

            VBLog.Message("Fragments Harmony patches were successful");
        }
    }
}
