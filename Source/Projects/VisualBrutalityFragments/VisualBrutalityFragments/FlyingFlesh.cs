using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses;

namespace VisualBrutalityFragments
{
    internal class FlyingFlesh : Projectile
    {
        private ThingDef fleshDef;
        private Rot4 rotation;
        private float currentDegrees;

        private float CurrentAngles => this.currentDegrees * Mathf.Deg2Rad;
        public ThingDef FleshDef
        {
            get => this.fleshDef;
            set
            {
                if (value != null)
                    this.fleshDef = value;
            }
        }

        public override Graphic Graphic => fleshDef.graphic;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            this.rotation = Rot4.Random;
            currentDegrees = 0;
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            if (VisualBrutalityMod.Settings.GenerateFlesh)
            {
                GenSpawn.TrySpawn(this.FleshDef, this.Position, MapHeld, Rot4.Random, out Thing flesh);
                flesh?.SetForbidden(true);
            }
            base.Destroy(mode);
        }


        protected float DistanceCoveredFractionFadeOut
        {
            get
            {
                float ratio = 1 - (float)this.ticksToImpact / this.StartingTicksToImpact;
                return ratio / 11 - 1 / (ratio * 10 + 1) + 1;
            }
        }

        public override Vector3 ExactPosition
        {
            get
            {
                Vector3 vector = (this.destination - this.origin).Yto0() * this.DistanceCoveredFractionFadeOut;
                return this.origin.Yto0() + vector + Vector3.up * this.def.Altitude;
            }
        }

        public override Vector2 DrawSize => fleshDef.graphicData.drawSize;

        protected override void Tick()
        {
            base.Tick();
            this.currentDegrees += Time.deltaTime * 3600;
        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            this.Graphic.Draw(drawLoc, this.rotation, this, this.CurrentAngles);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref fleshDef, "fleshDef");
        }
    }
}
