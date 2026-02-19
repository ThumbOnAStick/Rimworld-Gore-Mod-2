using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using VisualBrutalityCorpses.Patches;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityHead
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
                harmony = (MetaPatches.harmony = new Harmony("thumb.VBHead"));
            }
            try
            {
                MetaPatches.harmony = harmony;
                MissingNeckPatch.PatchHarmony();
            }
            catch (Exception e)
            {
                Dialog_MessageBox box = new Dialog_MessageBox($"[Visual Brutalty] a severe error occured while trying to patch harmony, you can report this bug on bug report thread. {e}");
                Find.WindowStack.Add(box);
            }

            VBLog.Message("Head Harmony patches were successful");
        }
    }
}
