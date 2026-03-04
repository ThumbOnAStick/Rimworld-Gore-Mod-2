using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityFragments
{
    [StaticConstructorOnStartup]
    public class VBFragmentPool
    {
        public static readonly List<ThingDef> AllBrainParts;

        static VBFragmentPool()
        {
            AllBrainParts = DefDatabase<ThingDef>.AllDefs.
                Where(x => x.defName.ToLower().Contains("filth_brainpart")).ToList();
            if(AllBrainParts.Count < 1)
            {
                VBLog.ErrorSevere("Brain part filth def is not found!!!");
            }
        }
    }
}
