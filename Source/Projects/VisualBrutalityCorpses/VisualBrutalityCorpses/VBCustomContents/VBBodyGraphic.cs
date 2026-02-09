using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.VBCustomContents
{
    public static class VBBodyGraphic
    {


        public static Texture2D GoreMaskFor(Pawn pawn)
        {
            if(pawn == null) return null;
            var recorder = pawn.TryGetComp<CompDeathRecorder>();
            if (recorder == null) return null;
            if (pawn.story == null) return null;
            try
            {
                if (recorder.LastHitDamage == null) return null;
                return VBContentDatabase.GetCrushMaskInRot(pawn.story.bodyType, pawn.Rotation);
            }catch(Exception e)
            {
                VBLog.ErrorSevere(e.Message); return null;
            }
        }



    }
}
