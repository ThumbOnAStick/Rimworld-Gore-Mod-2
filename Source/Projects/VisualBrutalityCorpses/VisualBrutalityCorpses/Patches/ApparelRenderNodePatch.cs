using HarmonyLib;
using KTrie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;
using VisualBrutalityCorpses.Compatibility;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.RenderNode;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.Patches
{
    internal static class ApparelRenderNodePatch
    {
        public static void PatchHarmony()
        {
            MethodInfo original = AccessTools.Method(typeof(DynamicPawnRenderNodeSetup_Apparel), "GetDynamicNodes");
            HarmonyMethod postfix = new HarmonyMethod(typeof(ApparelRenderNodePatch).GetMethod("Postfix"));
            MetaPatches.harmony.Patch(original, null, postfix);
        }

        private static IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> AddSubworkers(IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> __result)
        {
            foreach (var item in __result)
            {
                if (item.Item1 != null)
                {
                    var node = item.Item1;
                    if (node.Props.subworkerClasses == null)
                    {
                        node.Props.subworkerClasses = new List<Type>();
                    }
                    node.Props.subworkerClasses?.AddDistinct(typeof(PawnRenderSubWorker_Various));
                    yield return (node, item.Item2);
                }
            }

            yield break;
        }
        public static void Postfix( object[] __args, ref IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> __result)
        {
            if (__args[0] == null) return;
            if (!(__args[0] is Pawn pawn)) return;
            CompDeathRecorder compDeathRecorder = pawn.TryGetComp<CompDeathRecorder>();
            if (compDeathRecorder == null) return;
            __result = AddSubworkers(__result);
 
        }
    }
}

