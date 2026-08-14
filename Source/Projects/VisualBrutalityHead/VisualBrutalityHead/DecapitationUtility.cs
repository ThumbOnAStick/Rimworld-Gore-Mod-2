using LudeonTK;
using RimWorld;
using System;
using UnityEngine;
using Verse;
using Verse.Noise;
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
            if (pawn?.Map == null || pawn.MapHeld == null || pawn.story == null) return;
            try
            {
                IntRange intRange = new IntRange(3, 5);
                int scatter = intRange.RandomInRange;
                bool found = CellFinder.TryRandomClosewalkCellNear(pawn.PositionHeld, pawn.MapHeld, scatter, out IntVec3 intVec);
                if (found)
                {
                    LocalTargetInfo localTargetInfo = new LocalTargetInfo(intVec);
                    Thing spawned = GenSpawn.Spawn(VBHeadDefOf.HeadProjectile, pawn.Position, pawn.Map, WipeMode.Vanish);
                    if (!(spawned is HeadProjectile projectile_FlyingHead)) return;
                    projectile_FlyingHead.HeadInfoo = new HeadInfo(pawn.gender, pawn.Map, pawn.story.SkinColor, pawn, pawn.story.headType, Vector2.one);
                    if (Compatibility_HAR.IsHARActive() && Compatibility_HAR.HasHeadGraphics(pawn, out string path, out Vector2 drawSize))
                    {
                        VBLog.Message($"HAR head graphic found, path: {path}");
                        projectile_FlyingHead.HeadInfoo.HARPath = path;
                        projectile_FlyingHead.HeadInfoo.DrawSize = drawSize * pawn.DrawSize;
                    }
                    projectile_FlyingHead.Launch(pawn, localTargetInfo, localTargetInfo, ProjectileHitFlags.None, false, null);
                }
            }
            catch (Exception ex)
            {
                VBLog.Error($"Failed to launch head for {pawn?.LabelShort}: {ex}");
            }
        }
    }
}
