using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Compatibility;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityHead
{
    public static class DecapitationUtility
    {
        [DebugAction("VB", "BreakNeck", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns)]
        public static void MakeBrokenLimbForHuman(Pawn pawn)
        {
            if (!pawn.def.race.Humanlike) return;
            Hediff missingNeck = HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, pawn);
            var neck = pawn.health.hediffSet.GetBodyPartRecord(BodyPartDefOf.Neck);
            if (neck == null) return;
            missingNeck.Part = neck;
            pawn.health.AddHediff(missingNeck);
        }

        public static void LaunchHead(Pawn pawn)
        {
            IntRange intRange = new IntRange(3, 5);
            int randomInRange = intRange.RandomInRange;
            if (pawn == null) return;
            if (pawn.Map == null || pawn.MapHeld == null) return;
            bool flag = !CellFinder.TryFindRandomCellNear(pawn.Position, pawn.Map, randomInRange, x => (x - pawn.Position).LengthHorizontalSquared >= 3, out IntVec3 intVec, 10);
            if (!flag)
            {
                LocalTargetInfo localTargetInfo = new LocalTargetInfo(intVec);
                HeadProjectile projectile_FlyingHead = (HeadProjectile)GenSpawn.Spawn(VBHeadDefOf.HeadProjectile, pawn.Position, pawn.Map, WipeMode.Vanish);
                projectile_FlyingHead.HeadInfoo = new HeadInfo(pawn.gender, pawn.Map, pawn.story.SkinColor, pawn, pawn.story.headType, Vector2.one);
                if(Compatibility_HAR.HasHeadGraphics(pawn, out string path, out Vector2 drawSize))
                {
                    VBLog.Message($"HAR head graphic found, path: {path}");
                    projectile_FlyingHead.HeadInfoo.HARPath = path;
                    projectile_FlyingHead.HeadInfoo.DrawSize = drawSize * pawn.DrawSize;

                }
                projectile_FlyingHead.Launch(pawn, localTargetInfo, localTargetInfo, ProjectileHitFlags.None, false, null);
            }
        }
    }
}
