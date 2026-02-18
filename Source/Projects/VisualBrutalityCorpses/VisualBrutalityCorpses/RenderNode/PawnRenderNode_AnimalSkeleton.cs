using RimWorld;
using System;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.RenderNode
{
    internal class PawnRenderNode_AnimalSkeleton : PawnRenderNode
    {
        public PawnRenderNode_AnimalSkeleton(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
        {
        }
        public override GraphicMeshSet MeshSetFor(Pawn pawn)
        {
            Graphic graphic = this.GraphicFor(pawn);
            if (graphic != null)
            {
                return MeshPool.GetMeshSetForSize(graphic.drawSize.x, graphic.drawSize.y);
            }
            return null;
        }

        public override Color ColorFor(Pawn pawn)
        {
            return ColorUtils.GetBloodColor(pawn);
        }

        public override Graphic GraphicFor(Pawn pawn)
        {
            if (!pawn.Dead) return null;
            if (pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Dessicated) return null;
            PawnKindLifeStage curKindLifeStage = pawn.ageTracker.CurKindLifeStage;
            if (curKindLifeStage.dessicatedBodyGraphicData == null) return null;
            Graphic graphic3;
            pawn.TryGetAlternate(out AlternateGraphic alternateGraphic, out int _);
            if (pawn.RaceProps.FleshType == FleshTypeDefOf.Insectoid)
            {
                Color dessicatedColorInsect = PawnRenderUtility.DessicatedColorInsect;
                if (pawn.gender != Gender.Female || curKindLifeStage.femaleDessicatedBodyGraphicData == null)
                {
                    graphic3 = curKindLifeStage.dessicatedBodyGraphicData.Graphic.GetColoredVersion(ShaderDatabase.Cutout, dessicatedColorInsect, dessicatedColorInsect);
                }
                else
                {
                    graphic3 = curKindLifeStage.femaleDessicatedBodyGraphicData.Graphic.GetColoredVersion(ShaderDatabase.Cutout, dessicatedColorInsect, dessicatedColorInsect);
                }
            }
            else if (pawn.gender != Gender.Female || curKindLifeStage.femaleDessicatedBodyGraphicData == null)
            {
                graphic3 = curKindLifeStage.dessicatedBodyGraphicData.GraphicColoredFor(pawn);
            }
            else
            {
                graphic3 = curKindLifeStage.femaleDessicatedBodyGraphicData.GraphicColoredFor(pawn);
            }
            if (alternateGraphic != null)
            {
                graphic3 = alternateGraphic.GetDessicatedGraphic(graphic3);
            }

            // Apply blood color using GetColoredVersion instead of direct assignment
            Color bloodColor = ColorUtils.GetBloodColor(pawn);
            graphic3 = graphic3.GetColoredVersion(graphic3.Shader, bloodColor, bloodColor);
            graphic3.drawSize = curKindLifeStage.bodyGraphicData.drawSize;
            graphic3 = GraphicDatabase.Get<Graphic_Multi>(
                graphic3.path, 
                ShaderDatabase.CutoutSkinOverlay, 
                graphic3.drawSize, 
                graphic3.color, 
                graphic3.colorTwo, 
                null, 
                pawn.Graphic.path);
            //Texture2D tex = graphic3.MatEast.mainTexture;
            return graphic3;
        }
    }
}
