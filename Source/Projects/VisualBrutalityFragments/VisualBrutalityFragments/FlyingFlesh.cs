using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityFragments
{
    internal class FlyingFlesh : Projectile
    {
        private ThingDef fleshDef;
        private Rot4 rotation;

        private Color bloodColor = Color.white;
        private float currentDegrees;
        private bool ascending = false;
        private bool isAnomalyFlesh = false;

        private Graphic graphicCached;

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

        public override Graphic Graphic { get
            {
                if (graphicCached == null)
                {
                    graphicCached = fleshDef.graphic;
                }
                if (!isAnomalyFlesh)
                {
                    return graphicCached;
                }
                else
                {
                    // Use dark red color for shamblers and other entities
                    Color color = new Color(.3f, 0f, 0f);
                    return graphicCached.GetColoredVersion(ShaderDatabase.Cutout, color, Color.white);
                }
            }
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            this.rotation = Rot4.Random;
            currentDegrees = 0;
        }

        /// <summary>
        /// Spawns filth when filth mode is enabled and it's anomly flesh
        /// </summary>
        /// <returns>Whether or not filth was spawned successfully</returns>
        private bool TrytoGenerateFilth()
        {
            if (VisualBrutalityMod.Settings.FilthMode || this.isAnomalyFlesh)
            {
                GenSpawn.TrySpawn(VBFragmentsDefOf.Filth_Flesh, this.Position, MapHeld, Rot4.Random, out Thing flesh);
                var filth = flesh as FilthFlesh;
                filth?.SetIsAnomaly(this.isAnomalyFlesh);
                filth?.SetOverrideColor(this.bloodColor);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Spawn flesh as item
        /// </summary>
        private void SpawnFleshItem()
        {
            GenSpawn.TrySpawn(this.FleshDef, this.Position, MapHeld, Rot4.Random, out Thing flesh);
            flesh?.TrySetForbidden(true);
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            if (VisualBrutalityMod.Settings.GenerateFlesh && !TrytoGenerateFilth())
            {
                SpawnFleshItem();
            }

            base.Destroy(mode);
        }


        protected float DistanceCoveredFractionFadeOut
        {
            get
            {
                float fraction = DistanceCoveredFraction;
                return fraction / 11 - 1 / (fraction * 10 + 1) + 1;
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

        protected override void Tick()
        {
            base.Tick();
            this.currentDegrees += Time.deltaTime * 3600;

        }

        float GetSizeMultiplier()
        {
            float distanceFrom05 = Mathf.Abs(0.5f - this.DistanceCoveredFraction);
            return Mathf.Max(1, 5f - (distanceFrom05 * 5f));
        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            //if (this.ascending && graphicCached != null)
            //{
            //   var drawSize = Vector2.one * GetSizeMultiplier();
            //}
            
            Graphic g = this.Graphic;
            g.Draw(drawLoc, this.rotation, this, this.CurrentAngles);
        }

        public void SetAscending(bool ascending)
        {
            this.ascending = ascending;
        }

        public void SetIsAnomaly(bool anomaly)
        {

            this.isAnomalyFlesh = anomaly;
        }

        public void SetBloodColor(Color color)
        {

            this.bloodColor = color;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref fleshDef, "fleshDef");
            Scribe_Values.Look(ref ascending, "ascending");
            Scribe_Values.Look(ref bloodColor, "bloodColor");
        }

    }
}
