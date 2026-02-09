using RimWorld;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.VBCustomContents;

namespace VisualBrutalityCorpses.Comps
{
    public class CompDeathRecorder: ThingComp
    {
        private DamageDef lastHitDamage = DamageDefOf.Blunt;
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
                return VBBodyGraphic.GoreMaskFor(SelfPawn);
            }
        }
        public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            base.PostPreApplyDamage(ref dinfo, out absorbed);
            if (SelfPawn.Dead) return;
            this.lastHitDamage = dinfo.Def;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Defs.Look(ref lastHitDamage, "lastHitDamage");
        }

        public void OnCorpseBurnt(DamageInfo info)
        {
            if(info.Def == DamageDefOf.Burn || info.Def == DamageDefOf.Flame)
            this.lastHitDamage = info.Def;
        }
  
    }
}
