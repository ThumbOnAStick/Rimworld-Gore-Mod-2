using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace VisualBrutalityCorpses.Comps
{
    public class CompProperties_DeathRecorder : CompProperties
    {
        public CompProperties_DeathRecorder() 
        {
            this.compClass = typeof(CompDeathRecorder);
        }
    }
}
