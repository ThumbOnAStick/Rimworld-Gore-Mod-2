using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;
using VisualBrutalityCorpses.Graphics;
using VisualBrutalityCorpses.Patches;

namespace VisualBrutalityHead
{
    public class MissingNeckPatch
    {
        public static void PatchHarmony()
        {
            MethodInfo original = AccessTools.Method(typeof(Hediff_MissingPart), "PostAdd");
            HarmonyMethod postfix = new HarmonyMethod(typeof(MissingNeckPatch).GetMethod("Postfix"));
            MetaPatches.harmony.Patch(original, null, postfix);
        }

        public static void Postfix(DamageInfo? dinfo, Hediff_MissingPart __instance)
        {
            if (__instance.pawn == null ||
                __instance.pawn.def.race == null ||
                !__instance.pawn.def.race.Humanlike) return;
            if (__instance.Part.def != BodyPartDefOf.Neck) return;

            // Drop head here
            var pawn = __instance.pawn;
            DecapitationUtility.LaunchHead(pawn);
        }
    }
}
