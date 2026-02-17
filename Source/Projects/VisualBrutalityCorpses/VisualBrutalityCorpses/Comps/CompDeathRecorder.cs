using RimWorld;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.VBCustomContents;

namespace VisualBrutalityCorpses.Comps
{
    public class CompDeathRecorder: ThingComp
    {
        private DamageDef lastHitDamage = DamageDefOf.Blunt;
        private float lasthitDamageAmount = 0;
        public Pawn SelfPawn => (Pawn)this.parent;

        public DamageDef LastHitDamage => this.lastHitDamage;

        public bool HasSpecialGoreTexture
        {
            get {
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
        public bool HasSpecialCorpseMask
        {
            get
            {
                if (lasthitDamageAmount > 50) return true;
                return lastHitDamage == DamageDefOf.Crush ||
                    lastHitDamage == DamageDefOf.Bite ||
                    lastHitDamage == DamageDefOf.Flame ||
                    lastHitDamage == DamageDefOf.AcidBurn ||
                    lastHitDamage == DamageDefOf.Bomb ||
                    lastHitDamage == DamageDefOf.Cut ||
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
        }

        public void OnCorpseBurnt(DamageInfo info)
        {
            if(info.Def == DamageDefOf.Burn || info.Def == DamageDefOf.Flame)
            this.lastHitDamage = info.Def;
        }
  
    }
}
