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
            if (__instance.pawn == null ||
                __instance.pawn.def.race == null ||
                !__instance.pawn.def.race.Humanlike) return;
            if (__instance.Part == null) return;
            if (__instance.Part.def == BodyPartDefOf.Head)
            {
                // Do head fragments here
            }
            if (__instance.Part.GetDirectChildParts().EnumerableNullOrEmpty() || !__instance.Part.parent.IsCorePart ) // Part has less than 1 sub parts
            {
                return;
            }
            DismembermentUtils.MakeFlyingFlesh(__instance.pawn);
            
        }
    }
}
