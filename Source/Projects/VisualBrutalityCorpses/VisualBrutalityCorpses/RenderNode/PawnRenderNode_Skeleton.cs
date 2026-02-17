using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Compatibility;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.RenderNode
{
    internal class PawnRenderNode_Skeleton : PawnRenderNode
    {
        public PawnRenderNode_Skeleton(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
            : base(pawn, props, tree)
        {
        }


        public override Color ColorFor(Pawn pawn)
        {
            return ColorUtils.GetSkeletonColor(pawn);
        }

        public override Graphic GraphicFor(Pawn pawn)
        {
            if (!pawn.Dead) return null;
            if(pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Dessicated) return null;
            if (!pawn.RaceProps.IsFlesh) return null;
            return GraphicDatabase.Get<Graphic_Multi>(pawn.story.bodyType.bodyDessicatedGraphicPath, ShaderDatabase.CutoutSkinOverlay, drawSize: pawn.DrawSize, color: ColorFor(pawn));

        }
    }
}
