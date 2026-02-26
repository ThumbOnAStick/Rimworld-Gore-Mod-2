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
            if (!(__args[0] is Pawn pawn) || !pawn.Dead) return;
            if (pawn.Drawer?.renderer?.HeadGraphic == null) return;
            if (Compatibility_HAR.IsHARActive() && Compatibility_HAR.IsPawnAlien(pawn))
            {
                Compatibility_HAR.ApplyHARHeadPrefix(ref __result, __instance, pawn);
            }
            CompDeathRecorder compDeathRecorder = pawn.TryGetComp<CompDeathRecorder>();
            if (compDeathRecorder == null) return;
            __result = new Graphic_MaskedSprite(__result, pawn, null, false);
        }
    }
}
