using RimWorld;
using UnityEngine;
using Verse;

namespace VisualBrutalityFragments
{
    internal class BrainPieceUtility
    {
        public static ThingDef RandomBrainPiece()
        {
            int rnd = Random.Range(0, 3);
            switch (rnd)
            {
                case 0:
                    return VBFragmentsDefOf.Filth_BrainPartA;
                case 1:
                    return VBFragmentsDefOf.Filth_BrainPartB;
                default:
                    return VBFragmentsDefOf.Filth_BrainPartC;
            }
        }
    }
}
