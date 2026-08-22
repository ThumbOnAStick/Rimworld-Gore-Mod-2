using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.Comps
{
    internal class CompGibsOverlay : ThingComp
    {
        private bool isGibSpilled = false;
        private bool isGibFromWest = false;
        private Color gibsColor;
        private TickTimer removeGibsTimer = new TickTimer();

        public Color GibsColor => this.gibsColor;
        public Color GibsColorSolid => new Color(this.gibsColor.r, this.gibsColor.g, this.gibsColor.b);
        public bool IsGibsFromWest => this.isGibFromWest;
        public Pawn SelfPawn => (Pawn)this.parent;

        public bool IsGibSpilled => this.isGibSpilled;

        /// <summary>
        /// Decrease gibs color's alpha
        /// </summary>
        /// <param name="amount">Fade amount</param>
        public void FadeColor(float amount)
        {
            gibsColor.a -= amount;
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
        public void GetGibsSpilledBy(Pawn otherPawn, IntVec3 dir)
        {
            if (otherPawn == null)
            {
                return;
            }
            this.gibsColor = ColorUtils.GetBloodColor(otherPawn);
            this.isGibSpilled = true;
            this.isGibFromWest = dir.x > 0;
            SelfPawn?.Drawer?.renderer.SetAllGraphicsDirty();
            removeGibsTimer?.Start(GenTicks.TicksGame, 60 * VisualBrutalityMod.Settings.GibsOverlayDuration, CleanGibs);
        }
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref isGibSpilled, "isGibSpilled");
            Scribe_Values.Look(ref isGibFromWest, "isGibFromWest");
            Scribe_Values.Look(ref gibsColor, "gibsColor", Color.white);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (this.removeGibsTimer != null)
                    this.removeGibsTimer.OnFinish = new Action(this.CleanGibs);
                else
                    removeGibsTimer = new TickTimer();
            }
        }

        public override void CompTickInterval(int delta)
        {
            if (isGibSpilled)
            {
                gibsColor.a -= delta / 5;
                removeGibsTimer.TickIntervalDelta();
            }
        }
    }
}
