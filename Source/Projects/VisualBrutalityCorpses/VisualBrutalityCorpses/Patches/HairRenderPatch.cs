using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Compatibility;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Graphics;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.Patches
{
    public static class HairRenderPatch
    {
        public static void PatchHarmony()
        {
            MethodInfo original = AccessTools.Method(typeof(PawnRenderNode_Hair), "GraphicFor");
            HarmonyMethod postfix = new HarmonyMethod(typeof(HairRenderPatch).GetMethod("Postfix"));
            MetaPatches.harmony.Patch(original, null, postfix);
        }
        public static void Postfix(object[] __args, ref Graphic __result, PawnRenderNode_Hair __instance)
        {
            if (__args[0] == null) return;
            if (!(__args[0] is Pawn pawn)) return;
            CompDeathRecorder compDeathRecorder = pawn.TryGetComp<CompDeathRecorder>();
            if (compDeathRecorder == null) return;
            if (compDeathRecorder.IsGibSpilled)
            {
                var color = compDeathRecorder.GibsColor;
                var not_trasparent = new Color(color.r, color.g, color.b, 1.0f);
                __result = __result.GetColoredVersion(ShaderDatabase.CutoutHair, not_trasparent, Color.white);
            }
        }
    }
}
