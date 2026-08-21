using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Verse;
using VisualBrutalityCorpses.Utils;
using VisualBrutalityCorpses.VBCustomContents;

namespace VisualBrutalityCorpses.Comps
{
    public class CompDeathRecorder : ThingComp
    {

        private static UnityEvent<CompDeathRecorder> pawnKilledEvent;
        public static UnityEvent<CompDeathRecorder> PawnKilledEvent
        {
            get
            {
                if (pawnKilledEvent != null) return pawnKilledEvent;
                pawnKilledEvent = new UnityEvent<CompDeathRecorder>();
                return pawnKilledEvent;
            }
            set => pawnKilledEvent = pawnKilledEvent ?? value;
        }
        private DamageDef lastHitDamage = DamageDefOf.Blunt;
        private float lasthitDamageAmount = 0;
        private bool burnt = false;
        private bool torsoDestroyed = false;
        private bool isGibSpilled = false;
        private bool isGibFromWest = false;
        private Color gibsColor;

        public bool Burnt => burnt;

        public bool TorsoDestroyed => torsoDestroyed;
        public Pawn SelfPawn => (Pawn)this.parent;
        public DamageDef LastHitDamage => this.lastHitDamage;
        public Color GibsColor => this.gibsColor;
        public Color GibsColorSolid => new Color(this.gibsColor.r, this.gibsColor.g, this.gibsColor.b);
        public bool IsGibsFromWest => this.isGibFromWest;

        public bool IsGibSpilled => this.isGibSpilled;


        public bool HasSpecialGoreTexture
        {
            get
            {
                return SelfPawn != null && SelfPawn.Dead;
            }
        }

        public Texture2D GetGoreTextureBody
        {
            get
            {
                if (SelfPawn == null) return null;
                return VBBodyGraphic.BodyGoreMaskFor(SelfPawn);
            }
        }

        public Texture2D GetGoreTextureHead
        {
            get
            {
                if (SelfPawn == null) return null;
                return VBBodyGraphic.HeadGoreMaskFor(SelfPawn);
            }
        }

        public Texture2D GetGoreMaskAnimal
        {
            get
            {
                return VBBodyGraphic.AnimalGoreMaskFor(SelfPawn);
            }
        }
        public bool HasSpecialCorpseMask
        {
            get
            {
                if (lasthitDamageAmount > 50 || burnt) return true;
                return lastHitDamage == DamageDefOf.Crush ||
                    lastHitDamage == DamageDefOf.Bite ||
                    lastHitDamage == DamageDefOf.Burn ||
                    lastHitDamage == DamageDefOf.AcidBurn ||
                    lastHitDamage == DamageDefOf.Bomb ||
                    lastHitDamage == DamageDefOf.Cut ||
                    lastHitDamage == DamageDefOf.Bullet ||
                    lastHitDamage == DamageDefOf.Stab ||
                    lastHitDamage == DamageDefOf.Scratch;
            }
        }


        /// <summary>
        /// Set death recorder burnt
        /// </summary>
        /// <param name="_burnt">value</param>
        public void SetBurnt(bool _burnt)
        {
            bool previous = this.Burnt;
            this.burnt = _burnt;
            if (burnt && !previous)
                this.SelfPawn?.Corpse?.TryGetComp<CompRottable>()?.RotImmediately(stage: RotStage.Dessicated);
        }

        /// <summary>
        /// Set torso destroyed to "true" inside death recorder
        /// </summary>
        /// <param name="_torsoDestroyed">value</param>
        public void SetTorsoDestroyed(bool _torsoDestroyed)
        {
            this.torsoDestroyed = _torsoDestroyed;
        }


        /// <summary>
        ///  Checks if the last hit destroys pawn's torso
        /// </summary>
        /// <param name="pawn">Target pawn</param>
        /// <param name="dInfo">Damage info</param>
        /// <returns></returns>
        bool ShouldSplitTorso(DamageInfo dinfo)
        {
            if (SelfPawn == null || SelfPawn.health == null || SelfPawn.health.hediffSet == null) return false;
            BodyPartRecord corePart = SelfPawn.health.hediffSet.GetNotMissingParts().First(x => x.IsCorePart);
            if (corePart == null) return false;
            if (dinfo.HitPart == null || dinfo.HitPart.def != corePart.def)
            {
                return false;
            }
            return dinfo.Amount > corePart.def.hitPoints;
        }

        /// <summary>
        /// Clean gibs and set pawn renderer dirty
        /// </summary>
        void CleanGibs()
        {
            this.isGibSpilled = false;
            SelfPawn?.Drawer?.renderer?.SetAllGraphicsDirty();
        }

        /// <summary>
        /// Get gibs from other pawn spilled on this pawn
        /// </summary>
        /// <param name="otherPawn"></param>
        void GetGibsSpilledBy(Pawn otherPawn, IntVec3 dir)
        {
            if (otherPawn == null)
            {
                return;
            }
            this.gibsColor = ColorUtils.GetBloodColor(otherPawn);
            this.isGibSpilled = true;
            this.isGibFromWest = dir.x > 0;
            SelfPawn.Drawer.renderer.SetAllGraphicsDirty();
           
        }

        /// <summary>
        /// Spill gibs on all neighbor cells when dead
        /// </summary>
        void SpillGibsOnNeighborPawns()
        {
            var map = SelfPawn.MapHeld;
            if(map == null)
            {
                return;
            }
            float radius = 1.5f;
            int num = GenRadial.NumCellsInRadius(radius);
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec = GenRadial.RadialPattern[i] + SelfPawn.PositionHeld;
                Pawn pawn = intVec.GetFirstPawn(map);
                if (pawn != null && !pawn.Dead && pawn.TryGetComp<CompDeathRecorder>() != null)
                {
                    pawn.TryGetComp<CompDeathRecorder>().GetGibsSpilledBy(SelfPawn, GenRadial.RadialPattern[i]);
                }
            }

        }

        /// <summary>
        /// Validate if torso is destroyed to perform torso destoryed VFX.
        /// </summary>
        /// <param name="dinfo"></param>
        void ValidateTorsoSplit(DamageInfo? dinfo = null)
        {
            if (dinfo == null)
            {
                return;
            }
            if (ShouldSplitTorso(dinfo.Value))
            {
                SetTorsoDestroyed(true);
                SpillGibsOnNeighborPawns();
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (isGibSpilled)
            {
                TickTimer t = new TickTimer();
                t.Start(GenTicks.TicksGame, 5000, CleanGibs);

            }
        }



        public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            base.PostPreApplyDamage(ref dinfo, out absorbed);
            if (SelfPawn.Dead) return;
            this.lastHitDamage = dinfo.Def;
            lasthitDamageAmount = dinfo.Amount;
        }


        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Defs.Look(ref lastHitDamage, "lastHitDamage");
            Scribe_Values.Look(ref lasthitDamageAmount, "lasthitDamageAmount");
            Scribe_Values.Look(ref burnt, "burnt");
            Scribe_Values.Look(ref torsoDestroyed, "torsoDestroyed");
            Scribe_Values.Look(ref isGibSpilled, "isGibSpilled");
            Scribe_Values.Look(ref isGibFromWest, "isGibFromWest");
            Scribe_Values.Look(ref gibsColor, "gibsColor", Color.white);

        }

        public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
        {
            base.Notify_Killed(prevMap, dinfo);
            this.SetBurnt(false); // Reset burn status everytime pawn dies.
            this.SetTorsoDestroyed(false); // Reset torso status everytime pawn dies.
            ValidateTorsoSplit(dinfo);
            PawnKilledEvent.Invoke(this);
        }

    }
}
