using RimWorld;
using UnityEngine;
using VEF.Weapons;
using Verse;
using VisualBrutalityCorpses.VBCustomContents;

namespace VisualBrutalityCorpses.Comps
{
    public class CompDeathRecorder : ThingComp
    {
        private DamageDef lastHitDamage = DamageDefOf.Blunt;
        private float lasthitDamageAmount = 0;
        private bool burnt = false;

        public bool Burnt => burnt;
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
                    lastHitDamage == DamageDefOf.Stab;
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
        }

        public void SetBurnt(bool _burnt)
        {
            this.burnt = _burnt;
            if(burnt) 
            this.SelfPawn?.Corpse?.TryGetComp<CompRottable>()?.RotImmediately(stage: RotStage.Dessicated);
        }

        public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
        {
            base.Notify_Killed(prevMap, dinfo);
            this.SetBurnt(false); // Reset burn status everytime pawn dies.
        }

    }
}
