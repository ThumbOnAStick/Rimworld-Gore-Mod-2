using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;
using VisualBrutalityCorpses.Compatibility;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Graphics;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.Patches
{
    public static class HeadRenderPatch
    {
        public static void PatchHarmony()
        {
            MethodInfo original = AccessTools.Method(typeof(PawnRenderNode_Head), "GraphicFor");
            HarmonyMethod postfix = new HarmonyMethod(typeof(HeadRenderPatch).GetMethod("Postfix"));
            MetaPatches.harmony.Patch(original, null, postfix);
        }
        public static void Postfix(object[] __args, ref Graphic __result, PawnRenderNode_Head __instance)
        {
            if (__args[0] == null) return;
            var pawn = __instance.tree.pawn;
            if (pawn == null || !pawn.Dead) return;
            if (!BodytypeCheck.IsSupported(pawn.RaceProps.body.defName)) return;
            if (Compatibility_HAR.IsHARActive() && Compatibility_HAR.IsPawnAlien(pawn))
            {
                Compatibility_HAR.ApplyHARHeadPrefix(ref __result, __instance, pawn);
            }
            CompDeathRecorder compDeathRecorder = pawn.TryGetComp<CompDeathRecorder>(); 
            if (compDeathRecorder == null) return;
            VBLog.Message("Try to apply head graphics");
            __result = MaskedSpriteHelper.CreateHeadSprite(__result, pawn);
        }
    }
}
