using System;
using RimWorld;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Compatibility;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityHead
{
    public class HeadGraphics
    {
        private static Graphic_Multi GetDefaultHeadGraphic(HeadInfo headInfo, Color color)
        {
            Shader shader = ShaderDatabase.Cutout;
            Graphic_Multi graphic_Multi = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(headInfo.HeadTypeDef.graphicPath, shader, headInfo.DrawSize, color);
            return graphic_Multi;
        }
        private static Graphic_Multi GetHARtHeadGraphic(HeadInfo headInfo, Color color)
        {
            Shader shader = ShaderDatabase.Cutout;
            try
            {
                Graphic_Multi graphic_Multi = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(headInfo.HARPath, shader, headInfo.DrawSize, color);
                return graphic_Multi;
            } catch (Exception e)
            {
                VBLog.Error($"Failed to get har head graphic: {e}");
                return GetDefaultHeadGraphic(headInfo, color);
            }
        }
        public static Graphic GetFlyingHeadGraphic(HeadInfo headInfo)
        {
            Graphic graphic;
            try
            {
                if (Compatibility_HAR.IsHARActive() && headInfo.HARPath != null)
                {
                    graphic = GetHARtHeadGraphic(headInfo, headInfo.SkinColor);
                }
                else
                {
                    graphic = GetDefaultHeadGraphic(headInfo, headInfo.SkinColor);
                }
            }
            catch (Exception e)
            {
                Log.Error($"GUD: Exception while generating head graphic:{e}");
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
