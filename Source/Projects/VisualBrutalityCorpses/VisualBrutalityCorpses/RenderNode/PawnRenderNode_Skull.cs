using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VEF.Graphics;
using Verse;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.RenderNode
{
    internal class PawnRenderNode_Skull : PawnRenderNode_Head
    {
        public PawnRenderNode_Skull(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
        {

        }

        public override Graphic GraphicFor(Pawn pawn)
        {
            if (!pawn.health.hediffSet.HasHead)
            {
                return null;
            }
            Graphic_Multi graphic_Multi = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(HeadTypeDefOf.Skull.graphicPath, ShaderDatabase.CutoutSkinOverlay, Vector2.one, ColorUtils.GetSkeletonColor(pawn));
            return graphic_Multi;
        }




    }
}
