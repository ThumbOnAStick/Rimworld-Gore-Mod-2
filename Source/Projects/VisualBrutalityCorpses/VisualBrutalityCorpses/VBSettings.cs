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

        public bool DrawSkeleton = false;
        public bool PlaySounds = false;
        public bool DrawIntestines = false;
        public bool OverrideCorpseTexture = false;
        public bool GenerateHeads = true;
        public bool GenerateFlesh = true;
        public bool FilthMode = false; // Fragments spawn filth on the ground instead

        public void Restore()
        {
            DrawSkeleton = false;
            PlaySounds = false;
            DrawIntestines = false;
            GenerateHeads = true;
            GenerateFlesh = true;
            OverrideCorpseTexture = true;
            FilthMode = false;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref DrawSkeleton, "DrawSkeleton", false);
            Scribe_Values.Look(ref PlaySounds, "PlaySounds", false);
            Scribe_Values.Look(ref GenerateFlesh, "GenerateFlesh", true);
            Scribe_Values.Look(ref GenerateHeads, "GenerateHeads", true);
            Scribe_Values.Look(ref DrawIntestines, "DrawIntestines", true);
            Scribe_Values.Look(ref OverrideCorpseTexture, "OverrideCorpseTexture", true);
            Scribe_Values.Look(ref FilthMode, "FilthMode", false);

            base.ExposeData();
        }
    }
}
