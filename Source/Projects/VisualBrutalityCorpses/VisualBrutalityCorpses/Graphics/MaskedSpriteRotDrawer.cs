using RimWorld;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Compatibility;
using VisualBrutalityCorpses.Comps;
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
            if (mask == null)
            {
                return baseMat;
            }

            if (this.materialCached != null && mask.Equals(maskCached))
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

        protected Material BuildTornMaterial(Material baseMat, Texture2D mask, Pawn pawn, Thing apparel = null, bool isBody = true)
        {
            try
            {
                bool isHead = isBody == false && apparel == null;
                bool shoudDrawSkull = isHead && VisualBrutalityMod.Settings.DrawSkeleton;
                var newMat = new Material(baseMat)
                {
                    shader = shoudDrawSkull ? VBContentDatabase.TestUnlitMixerShader : VBContentDatabase.TestUnlitShader,
                    mainTextureScale = baseMat.mainTextureScale,
                    mainTextureOffset = baseMat.mainTextureOffset
                };
                newMat.SetTexture("_Mask", mask);
                float revealAmount = apparel != null? 1f : 0.9f;
                Color color = apparel != null? Color.grey : ColorUtils.GetBloodColor(pawn);
                newMat.SetFloat("_RevealAmount", revealAmount);
                newMat.SetColor("_DamageLayerColor", color);
                Rot4 rot = ((pawn.GetPosture() == PawnPosture.Standing) ? pawn.Rotation : pawn.Drawer.renderer.LayingFacing());
                if (shoudDrawSkull) newMat.SetTexture("_TexTwo", VBContentDatabase.GetSkullTexture(rot));
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
