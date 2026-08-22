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
    internal class PawnRenderNode_GibsOverlay_Animal : PawnRenderNode_GibsOverlay_Body
    {
        private Vector2 drawsizeCached;
        protected override string GibsOverlayPath => "VBOverlays/Gibs/Animal/GibsOverlay";
        protected override string GraphicPath { get
            {
                if (!this.graphicPathCached.NullOrEmpty()) return this.graphicPathCached;
                if (this.tree.pawn == null) return "";
                if (!this.tree.pawn.IsAnimal) return "";
                PawnKindLifeStage curKindLifeStage = tree.pawn.ageTracker.CurKindLifeStage;
                string text;
                if (tree.pawn.gender != Gender.Female || curKindLifeStage.femaleGraphicData == null)
                {
                    text = curKindLifeStage.bodyGraphicData.texPath;
                    drawsizeCached = curKindLifeStage.bodyGraphicData.drawSize;
                }
                else
                {
                    text = curKindLifeStage.femaleGraphicData.texPath;
                    drawsizeCached = curKindLifeStage.femaleGraphicData.drawSize;
                }
                this.graphicPathCached = text;
                return text;
            }
        }

        public override GraphicMeshSet MeshSetFor(Pawn pawn)
        {
            PawnKindLifeStage curKindLifeStage = tree.pawn.ageTracker.CurKindLifeStage;
            if (tree.pawn.gender != Gender.Female || curKindLifeStage.femaleGraphicData == null)
            {
                drawsizeCached = curKindLifeStage.bodyGraphicData.drawSize;
            }
            else
            {
                drawsizeCached = curKindLifeStage.femaleGraphicData.drawSize;
            }
            return MeshPool.GetMeshSetForSize(drawsizeCached.x, drawsizeCached.y);
        }

        public override Color ColorFor(Pawn pawn)
        {
            if (deathRecorder == null)
                return base.ColorFor(pawn);
            return deathRecorder.GibsColor;
        }

        public PawnRenderNode_GibsOverlay_Animal(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
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
