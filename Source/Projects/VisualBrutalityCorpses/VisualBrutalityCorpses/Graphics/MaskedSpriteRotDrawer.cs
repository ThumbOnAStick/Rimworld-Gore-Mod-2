using RimWorld;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Compatibility;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Defs;
using VisualBrutalityCorpses.Utils;
using VisualBrutalityCorpses.VBCustomContents;

namespace VisualBrutalityCorpses.Graphics
{
    internal class MaskedSpriteRotDrawer
    {
        public Material GetMaterial(Material baseMat, Texture2D mask, Pawn pawn, Thing apparel = null, bool isBody = true)
        {
            if (mask == null)
            {
                VBLog.Error($"No gore mask found for ${pawn.ThingID}");
                return baseMat;
            }

            return BuildSpecialMaterial(baseMat, mask, pawn, apparel, isBody);
        }

        private bool ShouldDrawIntestines(Pawn pawn)
        {
            return false;
            //if (!VisualBrutalityMod.Settings.DrawIntestines) return false;
            //if (!pawn.def.race.Humanlike) return false;
            //var recorder = pawn.TryGetComp<CompDeathRecorder>();
            //if (recorder == null) return false;
            //return recorder.TorsoDestroyed;
        }

        /// <summary>
        /// Build special material based on base mat
        /// </summary>
        /// <param name="baseMat">The base material</param>
        /// <param name="mask">Material mask</param>
        /// <param name="pawn">Target pawn</param>
        /// <param name="apparel">Target apparel</param>
        /// <param name="isBody">Is drawing body or not</param>
        /// <returns>The modified material</returns>
        private Material BuildSpecialMaterial(Material baseMat, Texture2D mask, Pawn pawn, Thing apparel = null, bool isBody = true)
        {
            try
            {

                bool isHead = isBody == false && apparel == null;
                bool shouldDrawSkull = apparel == null && isHead && VisualBrutalityMod.Settings.DrawSkeleton;
                bool shouldDrawSkeleton = apparel == null && isBody && VisualBrutalityMod.Settings.DrawSkeleton;
                bool shouldDrawIntestines = ShouldDrawIntestines(pawn) && isBody && apparel == null;
                bool shouldUseMixer = !pawn.IsAnimal && (shouldDrawIntestines || shouldDrawSkull || shouldDrawSkeleton) ;
                var newMat = new Material(baseMat)
                {
                    color = baseMat.color,
                    shader = shouldUseMixer ? VBContentDatabase.TestUnlitMixerShader : VBContentDatabase.TestUnlitShader,
                    mainTextureScale = baseMat.mainTextureScale,
                    mainTextureOffset = baseMat.mainTextureOffset
                };
                newMat.SetTexture("_Mask", mask);
                Color color = apparel != null? Color.grey : ColorUtils.GetBloodColor(pawn);
                newMat.SetFloat("_FadeStrength", 5.0f);
                newMat.SetFloat("_MixStrength", 10.0f);
                newMat.SetColor("_DamageLayerColor", color);
                newMat.SetColor("_MixColor", color);
                if (pawn.IsAnimal)
                {
                    return newMat;
                }

                Rot4 rot = ((pawn.GetPosture() == PawnPosture.Standing) ? pawn.Rotation : pawn.Drawer.renderer.LayingFacing());
                if (shouldDrawSkull)
                {
                    newMat.SetTexture("_TexTwo", VBContentDatabase.GetSkullTexture(rot));
                }
                else if (shouldDrawSkeleton && pawn.story.bodyType != null)
                {
                    //VBLog.Message();
                    newMat.SetTexture("_TexTwo", VBContentDatabase.GetSkeletonTexture(pawn.story.bodyType, rot));
                }
                //if (shouldDrawIntestines) newMat.SetTexture("_TexTwo", IntestinesUtils.GetIntestinesForBodyType(pawn.story?.bodyType));

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
