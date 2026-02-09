using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
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

        public Material GetMaterial(Material baseMat, Texture2D mask, Pawn pawn = null, Thing apparel = null)
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
            return this.materialCached = BuildTornMaterial(baseMat, mask);
        }

        protected Material BuildTornMaterial(Material baseMat, Texture2D mask, Pawn pawn = null, Thing apparel = null)
        {
            try
            {
                var newMat = new Material(baseMat)
                {
                    shader = VBContentDatabase.TestUnlitShader
                };

              
                newMat.SetTexture("_Mask", mask);
                //newMat.SetFloat("_RevealAmount", ApparelDamageVisualsMod.Settings.HoleSize);
                newMat.SetColor("_DamageLayerColor", Color.grey);

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
