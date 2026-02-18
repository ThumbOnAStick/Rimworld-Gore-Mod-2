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

        public bool DrawSkeleton;
        public override void ExposeData()
        {
            Scribe_Values.Look(ref DrawSkeleton, "DrawSkeleton", false);
            base.ExposeData();
        }
    }
}
