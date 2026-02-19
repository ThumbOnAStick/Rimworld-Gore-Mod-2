using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace VisualBrutalityHead
{
    public class HeadGraphics
    {
        private static Graphic_Multi GetDefaultHeadGraphic(HeadTypeDef headType, Color color)
        {
            Shader shader = ShaderDatabase.Cutout;
             Graphic_Multi graphic_Multi = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(headType.graphicPath, shader, Vector2.one, color);
            return graphic_Multi;
        }

        public static Graphic GetFlyingHeadGraphic(HeadInfo headInfo)
        {
            Pawn pawn = headInfo.Pawn;
            bool hasHead = pawn != null && pawn.Drawer.renderer.renderTree.HeadGraphic != null;
            Graphic graphic;
            try
            {
                if (hasHead)
                {
                    graphic = pawn.story.headType.GetGraphic(pawn, headInfo.SkinColor);
                }
                else
                {
                    graphic = GetDefaultHeadGraphic(headInfo.HeadTypeDef, headInfo.SkinColor);
                }
            }
            catch (Exception e)
            {
                Log.Error($"GUD: Exception while generating flying head:{e}");
                return null;
            }

            return graphic;
        }

        public static void DrawHairAndBeard(HeadInfo headInfo, Thing thing, Vector3 hairDrawLoc, Vector2 drawSize, float rotation)
        {
            if (headInfo == null)
            {
                return;
            }

            HairDef hairDef = headInfo.HairDef;
            BeardDef beardDef = headInfo.BeardDef;
            Color hairColor = headInfo.HairColor;
            Color bearedColor = headInfo.BeardColor;

            if (hairDef != null && hairDef != HairDefOf.Bald)
            {
                Graphic graphic = GraphicDatabase.Get<Graphic_Multi>(hairDef.texPath, ShaderDatabase.CutoutHair, Vector2.one, hairColor);
                graphic.drawSize = drawSize;
                graphic.Draw(hairDrawLoc, headInfo.Facing, thing, rotation);
            }

            if (beardDef != null && beardDef != BeardDefOf.NoBeard)
            {
                Graphic graphic = GraphicDatabase.Get<Graphic_Multi>(beardDef.texPath, ShaderDatabase.CutoutHair, Vector2.one, bearedColor);
                graphic.drawSize = drawSize;
                graphic.Draw(hairDrawLoc, headInfo.Facing, thing, rotation);
            }
        }
    }
}
