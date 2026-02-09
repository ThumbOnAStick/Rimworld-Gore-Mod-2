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
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Graphics;

namespace VisualBrutalityCorpses.Patches
{
    public static class BodyRenderPatch
    {
        public static void PatchHarmony()
        {
            MethodInfo original = AccessTools.Method(typeof(PawnRenderNode_Body), "GraphicFor");
            HarmonyMethod postfix = new HarmonyMethod(typeof(BodyRenderPatch).GetMethod("Postfix"));
            MetaPatches.harmony.Patch(original, null, postfix);
        }
        public static void Postfix(object[] __args, ref Graphic __result, PawnRenderNode_Body __instance)
        {
            if (__args[0] == null) return;
            if (!(__args[0] is Pawn pawn) || !pawn.Dead) return;
            CompDeathRecorder compDeathRecorder = pawn.TryGetComp<CompDeathRecorder>();
            if (compDeathRecorder == null) return;
            __result =  new Graphic_MaskedSprite(__result, pawn);
        }
    }
}
