using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Verse;
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
                if(pawnKilledEvent != null) return pawnKilledEvent;
                pawnKilledEvent = new UnityEvent<CompDeathRecorder>();
                return pawnKilledEvent;
            } 
            set => pawnKilledEvent = pawnKilledEvent ?? value;
        }
        private DamageDef lastHitDamage = DamageDefOf.Blunt;
        private float lasthitDamageAmount = 0;
        private bool burnt = false;
        private bool torsoDestroyed = false;

        public bool Burnt => burnt;

        public bool TorsoDestroyed => torsoDestroyed;
        public Pawn SelfPawn => (Pawn)this.parent;
        public DamageDef LastHitDamage => this.lastHitDamage;

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

        bool TorsoCheck(Hediff h)
        {
            BodyPartRecord part = h.Part;
            return ((part != null) ? part.def : null) == BodyPartDefOf.Torso;
        }


        bool ShouldSplitTorso(Pawn pawn)
        {
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            if (hediffs != null && hediffs.Count >= 1 && hediffs.Any(TorsoCheck))
            {
                float severity = hediffs.Where(TorsoCheck).MaxBy(h => h.tickAdded).Severity;
                if (severity > (float)BodyPartDefOf.Torso.hitPoints)
                {
                    return true;
                }
            }
            return false;
        }

        void ValidateTorsoSplit(Pawn pawn)
        {
            if (ShouldSplitTorso(pawn))
            {
                this.SetTorsoDestroyed(true);
            }
        }

        public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
        {
            base.Notify_Killed(prevMap, dinfo);
            this.SetBurnt(false); // Reset burn status everytime pawn dies.
            this.SetTorsoDestroyed(false); // Reset torso status everytime pawn dies.
            ValidateTorsoSplit(this.SelfPawn);
            PawnKilledEvent.Invoke(this);
        }

    }
}
