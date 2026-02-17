using System;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Compatibility;
using VisualBrutalityCorpses.Utils;
using VisualBrutalityCorpses.VBCustomContents;

namespace VisualBrutalityCorpses.Graphics
{
    internal class MaskedSpriteRotDrawer
    {
        Texture2D maskCached;
        Material materialCached;
        protected enum EdgeDirection { North, South, East, West }

        public MaskedSpriteRotDrawer()
        {
            maskCached = null;
        }


        public Material GetMaterial(Material baseMat, Texture2D mask, Pawn pawn, Thing apparel = null)
        {
            if (this.materialCached != null && maskCached.Equals(mask))
            {
                return this.materialCached;
            }

            if (!UnityData.IsInMainThread)
            {
                return baseMat;
            }

            this.maskCached = mask;
            return this.materialCached = BuildTornMaterial(baseMat, mask, pawn, apparel);
        }

        private Color GetPawnBloodColorDefault(Pawn pawn)
        {
            return pawn.def.race.BloodDef != null ? pawn.def.race.BloodDef.graphicData.color : Color.grey;
        }

        protected Material BuildTornMaterial(Material baseMat, Texture2D mask, Pawn pawn, Thing apparel = null)
        {
            try
            {
                var newMat = new Material(baseMat)
                {
                    shader = VBContentDatabase.TestUnlitShader
                };

              
                newMat.SetTexture("_Mask", mask);
                float revealAmount = apparel != null? 1f : 0.9f;
                Color color = apparel != null? Color.grey : ColorUtils.GetBloodColor(pawn);
                newMat.SetFloat("_RevealAmount", revealAmount);
                newMat.SetColor("_DamageLayerColor", color);

                return newMat;
            }
            catch (Exception ex)
            {
                VBLog.Error(ex.ToString());
            }

            return baseMat;
        }
    }
}
