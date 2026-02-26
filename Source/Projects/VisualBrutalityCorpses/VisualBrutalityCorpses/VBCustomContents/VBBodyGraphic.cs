using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VEF.Weapons;
using Verse;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Defs;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.VBCustomContents
{
    public static class VBBodyGraphic
    {



        private static Rot4 GetRealRotation(Pawn pawn)
        {
            return pawn.Corpse != null ? pawn.Drawer.renderer.LayingFacing() : pawn.Rotation;
        }

        private static Texture2D GetGoreMask(Pawn pawn, Func<GoreMaskDef, Texture2D> getMask, Func<Texture2D> getSpecialMask = null, bool requireStory = true)
        {
            Texture2D emptyTex = Texture2D.whiteTexture;
            if (pawn == null) return emptyTex;
            var recorder = pawn.TryGetComp<CompDeathRecorder>();
            if (recorder == null) return emptyTex;
            if (requireStory && pawn.story == null) return emptyTex;
            try
            {
                if (recorder.Burnt) return emptyTex;
                if (recorder.LastHitDamage == null) return emptyTex;
                if (getSpecialMask != null)
                {
                    var special = getSpecialMask();
                    if (special != null) return special;
                }
                bool isCutOrStab = recorder.LastHitDamage == DamageDefOf.Stab || recorder.LastHitDamage == DamageDefOf.Cut;
                if(isCutOrStab) 
                    return getMask(VBDefOf.CutMask);
                if(recorder.LastHitDamage == DamageDefOf.Bullet)
                    return getMask(VBDefOf.ShotMask);
                return getMask(VBDefOf.CrushMask);

            }
            catch (Exception e)
            {
                VBLog.ErrorSevere(e.Message); return emptyTex;
            }
        }


        public static Texture2D BodyGoreMaskFor(Pawn pawn)
        {
            return GetGoreMask(
                pawn,
                mask => mask.GetBodyMaskInRot(pawn.story.bodyType, GetRealRotation(pawn)),
                () => pawn.TryGetComp<CompDeathRecorder>().TorsoDestroyed ? VBContentDatabase.GetSplitInHalfMask() : null);
        }

        public static Texture2D HeadGoreMaskFor(Pawn pawn)
        {
            return GetGoreMask(
                pawn,
                mask => mask.GetHeadMaskInRot(GetRealRotation(pawn)));
        }

        public static Texture2D AnimalGoreMaskFor(Pawn pawn)
        {
            return GetGoreMask(
                pawn,
                mask => mask.GetAnimalCorpseMaskInRot(GetRealRotation(pawn)),
                null,
                requireStory: false);
        }
    }
}
