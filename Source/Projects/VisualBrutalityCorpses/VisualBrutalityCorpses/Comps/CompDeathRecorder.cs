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
        private Color gibsColor = Color.white;

        public bool Burnt => burnt;

        public bool TorsoDestroyed => torsoDestroyed;
        public Pawn SelfPawn => (Pawn)this.parent;
        public DamageDef LastHitDamage => this.lastHitDamage;
        public Color GibsColor => this.gibsColor;

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
            Scribe_Values.Look(ref gibsColor, "gibsColor");

        }

        public void SetBurnt(bool _burnt)
        {
            bool previous = this.Burnt;
            this.burnt = _burnt;
            if (burnt && !previous)
                this.SelfPawn?.Corpse?.TryGetComp<CompRottable>()?.RotImmediately(stage: RotStage.Dessicated);
        }

        public void SetTorsoDestroyed(bool _torsoDestroyed)
        {
            this.torsoDestroyed = _torsoDestroyed;
        }


        /// <summary>
        ///  Checks if the last hit destroys pawn's torso
        /// </summary>
        /// <param name="pawn"></param>
        /// <param name="dInfo"></param>
        /// <returns></returns>
        bool ShouldSplitTorso(DamageInfo dinfo)
        {
            if (dinfo.HitPart == null || dinfo.HitPart.def != BodyPartDefOf.Torso)
            {
                VBLog.Message("Should not split torso");
                return false;
            }
            return dinfo.Amount > BodyPartDefOf.Torso.hitPoints;
        }

        void CleanGibs()
        {
            this.isGibSpilled = false;
        }

        /// <summary>
        /// Get gibs from other pawn spilled on this pawn
        /// </summary>
        /// <param name="otherPawn"></param>
        void GetGibsSpilledBy(Pawn otherPawn)
        {
            if (otherPawn == null)
            {
                return;
            }
            this.gibsColor = ColorUtils.GetBloodColor(otherPawn);
            this.isGibSpilled = true;
            SelfPawn.Drawer.renderer.EnsureGraphicsInitialized();
            //Task.Delay(5000).ContinueWith(t => CleanGibs());
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
                    pawn.TryGetComp<CompDeathRecorder>().GetGibsSpilledBy(SelfPawn);
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
