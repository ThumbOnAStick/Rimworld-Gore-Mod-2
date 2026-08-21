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

        protected override string GraphicPath { get
            {
                if (this.tree.pawn == null) return "";
                if (!this.tree.pawn.IsAnimal) return "";
                PawnKindLifeStage curKindLifeStage = tree.pawn.ageTracker.CurKindLifeStage;
                string text;
                if (tree.pawn.gender != Gender.Female || curKindLifeStage.femaleGraphicData == null)
                {
                    text = curKindLifeStage.bodyGraphicData.texPath;
                }
                else
                {
                    text = curKindLifeStage.femaleGraphicData.texPath;
                }
                return text;
            }
        }



        public PawnRenderNode_GibsOverlay_Animal(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
            : base(pawn, props, tree)
        {

        }

        public override Graphic GraphicFor(Pawn pawn)
        {
            if (deathRecorder == null) return null;
            //if (GraphicPath.NullOrEmpty()) return null;
            PawnKindLifeStage curKindLifeStage = pawn.ageTracker.CurKindLifeStage;
            Graphic graphic;
            if (pawn.gender != Gender.Female || curKindLifeStage.femaleGraphicData == null)
            {
                graphic = curKindLifeStage.bodyGraphicData.Graphic;
            }
            else
            {
                graphic = curKindLifeStage.femaleGraphicData.Graphic;
            }

            VBLog.Message($"Draw size: {graphic.drawSize}");

            return GraphicDatabase.Get<Graphic_Multi>(GibsOverlayPath, ShaderDatabase.Cutout, drawSize: graphic.drawSize, deathRecorder.GibsColor, Color.white, null, GraphicPath);

        }


    }
}
