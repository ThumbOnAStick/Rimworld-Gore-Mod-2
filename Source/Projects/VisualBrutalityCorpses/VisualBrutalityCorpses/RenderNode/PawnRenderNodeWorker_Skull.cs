using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace VisualBrutalityCorpses.RenderNode
{
    public class PawnRenderNodeWorker_Skull : PawnRenderNodeWorker_Overlay
    {
        protected override PawnOverlayDrawer OverlayDrawer(Pawn pawn)
        {
            return pawn.Drawer.renderer.WoundOverlays;
        }
    }
}
