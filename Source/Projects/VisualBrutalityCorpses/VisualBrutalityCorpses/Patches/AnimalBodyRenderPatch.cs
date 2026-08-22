using HarmonyLib;
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
    public class AnimalBodyRenderPatch
    {
        public static void PatchHarmony()
        {
            MethodInfo original = AccessTools.Method(typeof(PawnRenderNode_AnimalPart), "GraphicFor");
            HarmonyMethod postfix = new HarmonyMethod(typeof(AnimalBodyRenderPatch).GetMethod("Postfix"));
            MetaPatches.harmony.Patch(original, null, postfix);
        }
        public static void Postfix(object[] __args, ref Graphic __result, PawnRenderNode_AnimalPart __instance)
        {
            if (__args[0] == null) return;
            if (!(__args[0] is Pawn pawn) || !pawn.Dead) return;
            CompDeathRecorder compDeathRecorder = pawn.TryGetComp<CompDeathRecorder>();
            if (compDeathRecorder == null) return;
            VBLog.Message("Found animal gore graphic");
            __result = MaskedSpriteHelper.CreateBodySprite(__result, pawn);
        }
    }
}
