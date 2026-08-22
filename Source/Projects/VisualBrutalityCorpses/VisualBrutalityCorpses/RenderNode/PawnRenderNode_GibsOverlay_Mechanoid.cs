using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace VisualBrutalityCorpses.RenderNode
{
    internal class PawnRenderNode_GibsOverlay_Mechanoid : PawnRenderNode_GibsOverlay_Body
    {
        protected override string GibsOverlayPath => "VBOverlays/Gibs/Animal/GibsOverlay";
        private Vector2 drawsizeCached;
        protected override string GraphicPath
        {
            get
            {
                if (!this.graphicPathCached.NullOrEmpty()) return this.graphicPathCached;
                if (this.tree.pawn == null) return "";
                PawnKindLifeStage curKindLifeStage = tree.pawn.ageTracker.CurKindLifeStage;
                string text = curKindLifeStage.bodyGraphicData.texPath;
                drawsizeCached = curKindLifeStage.bodyGraphicData.drawSize;
                this.graphicPathCached = text;
                return text;
            }
        }

        public override GraphicMeshSet MeshSetFor(Pawn pawn)
        {
            PawnKindLifeStage curKindLifeStage = tree.pawn.ageTracker.CurKindLifeStage;
            drawsizeCached = curKindLifeStage.bodyGraphicData.drawSize;
            return MeshPool.GetMeshSetForSize(drawsizeCached.x, drawsizeCached.y);
        }

        public PawnRenderNode_GibsOverlay_Mechanoid(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
    : base(pawn, props, tree)
        {
        }

        public override Graphic GraphicFor(Pawn pawn)
        {
            if (!VisualBrutalityMod.Settings.EnableGibsOverlay) return null;
            if (deathRecorder == null) return null;
            if (GraphicPath.NullOrEmpty()) return null;
            return GraphicDatabase.Get<Graphic_Multi>(GibsOverlayPath, ShaderDatabase.CutoutSkinOverlay, drawSize: drawsizeCached, deathRecorder.GibsColorSolid, Color.white, null, GraphicPath);

        }
    }
}
