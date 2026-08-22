using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace VisualBrutalityCorpses
{
    public class VBSettings : ModSettings
    {

        public bool DrawSkeleton = false; // Draw skeleton or not 
        public bool PlaySounds = false; // Play sounds or not 
        public bool DrawIntestines = false; // Draw intestines or not
        public bool OverrideCorpseTexture = false; // Override corpse texture or not
        public float TorsoSplitThreshold = 0.75f; // Threshold of damage 
        public bool GenerateHeads = true; // Generate heads or not
        public bool GenerateFlesh = true; // Spawn fragment or not
        public bool FilthMode = false; // Fragments spawn filth on the ground instead
        public bool EnableGibsOverlay = false; // Draw gibs overlay or not
        public int GibsOverlayDuration = 10; // How long gibs overlay will last, in seconds.

        public void Restore()
        {
            DrawSkeleton = false;
            PlaySounds = false;
            DrawIntestines = false;
            GenerateHeads = true;
            TorsoSplitThreshold = 0.75f;
            GenerateFlesh = true;
            OverrideCorpseTexture = true;
            FilthMode = false;
            EnableGibsOverlay = false;
            GibsOverlayDuration = 10;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref DrawSkeleton, "DrawSkeleton", false);
            Scribe_Values.Look(ref PlaySounds, "PlaySounds", false);
            Scribe_Values.Look(ref GenerateFlesh, "GenerateFlesh", true);
            Scribe_Values.Look(ref GenerateHeads, "GenerateHeads", true);
            Scribe_Values.Look(ref DrawIntestines, "DrawIntestines", true);
            Scribe_Values.Look(ref OverrideCorpseTexture, "OverrideCorpseTexture", true);
            Scribe_Values.Look(ref TorsoSplitThreshold, "TorsoSplitThreshold", 0.75f);
            Scribe_Values.Look(ref FilthMode, "FilthMode", false);
            Scribe_Values.Look(ref EnableGibsOverlay, "EnableGibsOverlay", false);
            Scribe_Values.Look(ref GibsOverlayDuration, "GibsOverlayDuration", 10);

            base.ExposeData();
        }
    }
}
