using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Graphics;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.Patches
{
    public class CorpseBurntPatch
    {
        public static void PatchHarmony()
        {
            MethodInfo original = AccessTools.Method(typeof(ThingWithComps), nameof(ThingWithComps.PostApplyDamage));
            HarmonyMethod postfix = new HarmonyMethod(typeof(CorpseBurntPatch).GetMethod("Postfix"));
            MetaPatches.harmony.Patch(original, null, postfix);
        }

        public static void Postfix(DamageInfo dinfo, float totalDamageDealt, ThingWithComps __instance)
        {
            if (!(__instance is Corpse corpse)) return;
            if (dinfo.Def != DamageDefOf.Burn && dinfo.Def != DamageDefOf.Flame) return;
            if (corpse.InnerPawn == null) return;
            if (totalDamageDealt <= 1) return;
            CompDeathRecorder compDeathRecorder = corpse.InnerPawn.TryGetComp<CompDeathRecorder>();
            if (corpse.HitPoints < corpse.MaxHitPoints * 0.75f)
                compDeathRecorder?.SetBurnt(true);
        }
    }
}
