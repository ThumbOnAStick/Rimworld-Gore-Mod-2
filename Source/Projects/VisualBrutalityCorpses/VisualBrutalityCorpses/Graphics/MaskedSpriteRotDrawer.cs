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


        public Material GetMaterial(Material baseMat, Texture2D mask, Pawn pawn, Thing apparel = null, bool isBody = true)
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
            return this.materialCached = BuildTornMaterial(baseMat, mask, pawn, apparel, isBody);
        }

        private Color GetPawnBloodColorDefault(Pawn pawn)
        {
            return pawn.def.race.BloodDef != null ? pawn.def.race.BloodDef.graphicData.color : Color.grey;
        }

        protected Material BuildTornMaterial(Material baseMat, Texture2D mask, Pawn pawn, Thing apparel = null, bool isBody = true)
        {
            try
            {
                bool isHead = isBody == false && apparel == null;
                var newMat = new Material(baseMat)
                {
                    shader = isHead ? VBContentDatabase.TestUnlitMixerShader : VBContentDatabase.TestUnlitShader
                };

              
                newMat.SetTexture("_Mask", mask);
                float revealAmount = apparel != null? 1f : 0.9f;
                Color color = apparel != null? Color.grey : ColorUtils.GetBloodColor(pawn);
                newMat.SetFloat("_RevealAmount", revealAmount);
                newMat.SetColor("_DamageLayerColor", color);
                VBLog.Message($"{pawn.Name} rotation: {pawn.Rotation}");
                if (isHead) newMat.SetTexture("_TexTwo", VBContentDatabase.GetSkullTexture(pawn.Rotation));

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
