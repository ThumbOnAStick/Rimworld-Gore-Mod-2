using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Comps;

namespace VisualBrutalityCorpses.RenderNode
{
    public class PawnRenderWorker_Skeleton : PawnRenderNodeWorker
    {
        public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
        {
            if (node?.tree.pawn == null || parms.pawn == null) return false;
            if (!parms.pawn.Dead || !VisualBrutalityMod.Settings.DrawSkeleton) return false;
            return node.tree.pawn.TryGetComp<CompDeathRecorder>() != null;
        }

        public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
        {
            Vector3 offset = base.OffsetFor(node, parms, out pivot);
            Graphic primaryGraphic = node?.PrimaryGraphic;
            if (primaryGraphic == null) return offset;
            return offset + primaryGraphic.DrawOffset(parms.facing);
        }
    }
}
