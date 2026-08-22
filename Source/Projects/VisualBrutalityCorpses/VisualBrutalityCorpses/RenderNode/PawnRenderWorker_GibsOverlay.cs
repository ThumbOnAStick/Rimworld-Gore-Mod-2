using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.RenderNode
{
    public class PawnRenderWorker_GibsOverlay : PawnRenderNodeWorker
    {


        public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
        {

            Pawn pawn = parms.pawn;
            if (pawn == null || pawn.Dead)
            {
                return false;
            }
            var recorderComp = pawn.TryGetComp<CompDeathRecorder>();
            if (recorderComp == null)
            {
                return false;
            }
            return recorderComp.IsGibSpilled;
        }

        public override float LayerFor(PawnRenderNode node, PawnDrawParms parms)
        {
            return base.LayerFor(node, parms);
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
