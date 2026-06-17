using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Compatibility;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.RenderNode
{
    internal class PawnRenderNode_HumanSkeleton : PawnRenderNode
    {
        public PawnRenderNode_HumanSkeleton(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
            : base(pawn, props, tree)
        {
        }


        public override Color ColorFor(Pawn pawn)
        {
            return ColorUtils.GetSkeletonColor(pawn);
        }

        public override Graphic GraphicFor(Pawn pawn)
        {
            if (pawn == null || !pawn.Dead) return null;
            if (!pawn.RaceProps.IsFlesh) return null;

            CompDeathRecorder recorder = pawn.TryGetComp<CompDeathRecorder>();
            if (recorder == null || recorder.Burnt) return null;

            if (pawn.Drawer?.renderer != null && pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Dessicated) return null;

            string bodyDessicatedGraphicPath = pawn.story?.bodyType?.bodyDessicatedGraphicPath;
            if (bodyDessicatedGraphicPath.NullOrEmpty()) return null;

            return GraphicDatabase.Get<Graphic_Multi>(bodyDessicatedGraphicPath, ShaderDatabase.CutoutSkinOverlay, drawSize: pawn.DrawSize, color: ColorFor(pawn));

        }
    }
}
