using LudeonTK;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityFragments
{
    internal static class DismembermentUtils
    {
        [DebugAction("VB", "DestroyPawnLimb", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns)]
        public static void DestroyPawnLimb(Pawn pawn)
        {
            MakeFlyingFlesh(pawn, 90);
        }

        public static void MakeFlyingFlesh(Pawn pawn, float angle = -1)
        {
            try
            {
                if (pawn == null) return;
                var meat = pawn.RaceProps.meatDef;
                if (meat == null) return;
                if (pawn.def.race != null && !pawn.def.race.IsMechanoid) // When pawn is not a mechanoid, play gore sound.
                {

                }
                var map = pawn.MapHeld;
                if (map == null) return;
                SpawnFragment(pawn, map, meat, 3, new IntRange(1, 2).RandomInRange, angle);
            }
            catch (Exception ex)
            {
                VBLog.Error($"Failed to spawn a flying flesh for {pawn}, message: {ex}");
            }
        }

        [DebugAction("VB", "DestroyPawnHead", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns)]
        public static void TryDestroyHead(Pawn pawn)
        {
            if (!pawn.def.race.Humanlike) return;
            var head = pawn.health.hediffSet.GetBodyPartRecord(BodyPartDefOf.Head);
            if (head == null) return;
            pawn.TakeDamage(new DamageInfo(DamageDefOf.Crush, 100, hitPart: head));

        }

        /// <summary>
        /// Spawns flying head
        /// </summary>
        /// <param name="pawn">Target pawn</param>
        public static void MakeHeadFragments(Pawn pawn)
        {
            try
            {
                if (pawn == null) return;
                var map = pawn.MapHeld;
                if (map == null) return;
                for (int i = 0; i < 3; i++)
                {
                    SpawnFragment(pawn, map, VBFragmentsDefOf.Filth_BrainPartMeta, new IntRange(2, 4).RandomInRange, 1, -1, true);
                }
            }
            catch (Exception ex)
            {
                VBLog.Error($"Failed to spawn head fragments for {pawn}, message: {ex}");
            }
        }

        /// <summary>
        /// Spawns fragment from a pawn
        /// </summary>
        /// <param name="pawn">Target pawn</param>
        /// <param name="map">Target map</param>
        /// <param name="fleshDef">Def of the flesh</param>
        /// <param name="distance">How far the fragment will fly</param>
        /// <param name="scatter">The scatter of the fragment</param>
        /// <param name="angle">The angle of its movement</param>
        /// <param name="ascending">If the fragment is ascending or not</param>
        private static void SpawnFragment(Pawn pawn, Map map, ThingDef fleshDef, int distance, int scatter, float angle = -1, bool ascending = false)
        {
            IntVec3 pawnCell = pawn.Position;
            var rad = angle * Mathf.Deg2Rad;
            IntVec3 targetCell = angle == -1 ? pawnCell : pawnCell +
                new IntVec3((int)(Mathf.Cos(rad) * distance),
                0,
                (int)(Mathf.Sin(rad) * distance));
            bool found = CellFinder.TryRandomClosewalkCellNear(pawnCell, map, scatter, out IntVec3 intVec, (vec) => { return targetCell.DistanceTo(vec) < 5f; });
            if (!found) return;
            LocalTargetInfo dest = new LocalTargetInfo(intVec);
            FlyingFlesh fragment = (FlyingFlesh)GenSpawn.Spawn(VBFragmentsDefOf.FlyingFlesh, pawn.Position, map, WipeMode.Vanish);
            fragment.FleshDef = fleshDef;
            fragment.SetAscending(ascending);
            fragment.SetBloodColor(ColorUtils.GetBloodColor(pawn));
            fragment.SetIsAnomaly(pawn.RaceProps.IsAnomalyEntity || pawn.IsShambler);
            fragment.Launch(pawn, dest, dest, ProjectileHitFlags.None, false, null);
        }
        
        //TODO: Design actual torso fragment.
        /// <summary>
        /// Spawns fragment from torso
        /// </summary>
        /// <param name="comp"></param>
        public static void TrySpawnTorsoFragment(CompDeathRecorder comp)
        {
            if (comp == null) return;
            if (!comp.TorsoDestroyed) return;

        }
    }
}
