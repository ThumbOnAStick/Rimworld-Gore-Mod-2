using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace VisualBrutalityFragments
{
    internal class DismemberPatch
    {
        public static void PatchHarmony()
        {
            MethodInfo original = AccessTools.Method(typeof(Hediff_MissingPart), "PostAdd");
            HarmonyMethod postfix = new HarmonyMethod(typeof(DismemberPatch).GetMethod("Postfix"));
            MetaPatches.harmony.Patch(original, null, postfix);
        }

        public static void Postfix(DamageInfo? dinfo, Hediff_MissingPart __instance)
        {
            if (__instance.pawn == null) return;
            if (__instance.Part == null) return;
            if (dinfo != null && dinfo.Value.Def == DamageDefOf.SurgicalCut) return; // When it's surgical cut
            if (__instance.Part.def == BodyPartDefOf.Head)
            {
                // Do head fragments here
            }
            if ((__instance.Part.GetDirectChildParts().EnumerableNullOrEmpty()
                || !__instance.Part.parent.IsCorePart
                ) && __instance.Part.def != BodyPartDefOf.Arm) // Part has less than 1 sub parts
            {
                return;
            }
            DismembermentUtils.MakeFlyingFlesh(__instance.pawn);

        }
    }
}
