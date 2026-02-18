using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace VisualBrutalityCorpses.RenderNode
{
    public class PawnRenderWorker_Skeleton : PawnRenderNodeWorker
    {
        public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
        {
            return parms.pawn.Dead && VisualBrutalityMod.Settings.DrawSkeleton;
        }

        public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
        {
            return base.OffsetFor(node, parms, out pivot) + node.PrimaryGraphic.DrawOffset(parms.facing);
        }
    }
}
