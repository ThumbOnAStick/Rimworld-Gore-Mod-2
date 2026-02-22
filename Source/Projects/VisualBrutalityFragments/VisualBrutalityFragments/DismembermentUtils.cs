using LudeonTK;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityFragments
{
    internal static class DismembermentUtils
    {
        [DebugAction("VB", "DestroyPawnLimb", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns)]

        public static void DestroyPawnLimb(Pawn pawn)
        {
            //pawn.health.hediffSet.TryGetBodyPartRecord(BodyPartDefOf.Arm, out BodyPartRecord part);
            //var missingPartHediff = HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, pawn, part);
            //pawn.health.AddHediff(missingPartHediff);
            MakeFlyingFlesh(pawn);
        }
        public static void MakeFlyingFlesh(Pawn pawn)
        {
            try
            {
                if (pawn == null) return;
                var meat = pawn.RaceProps.meatDef;
                if (meat == null) return;
                if (pawn.def.race != null && !pawn.def.race.IsMechanoid) // When pawn is not a mechanoid, play gore sound.
                {

                }
                IntRange intRange = new IntRange(1, 3);
                int randomInRange = intRange.RandomInRange;
                var map = pawn.MapHeld;
                if (map == null) return;
                bool flag = !CellFinder.TryFindRandomCellNear(pawn.Position, map, randomInRange, null, out IntVec3 intVec, 10);
                if (!flag)
                {
                    LocalTargetInfo localTargetInfo = new LocalTargetInfo(intVec);
                    FlyingFlesh projectile_FlyingFlesh = (FlyingFlesh)GenSpawn.Spawn(VBFragmentsDefOf.FlyingFlesh, pawn.Position, map, WipeMode.Vanish);
                    projectile_FlyingFlesh.FleshDef = meat;
                    projectile_FlyingFlesh.Launch(pawn, localTargetInfo, localTargetInfo, ProjectileHitFlags.None, false, null);
                }

            }
            catch(Exception ex)
            {
                VBLog.Error($"Failed to spawn a flying flesh for {pawn}, message: {ex}");
            }
        }
    }
}
