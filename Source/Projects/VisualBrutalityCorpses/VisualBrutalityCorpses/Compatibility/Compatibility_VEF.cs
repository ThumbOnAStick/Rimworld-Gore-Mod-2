using System;
using UnityEngine;
using VEF.Genes;
using Verse;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.Compatibility
{
    public static class Compatibility_VEF
    {

        public static Color GetPawnBloodColor(Pawn pawn)
        {
            try
            {
                if (pawn.genes == null) return ColorUtils.GetDefaultBloodColor(pawn);
                foreach (var gene in pawn.genes.GenesListForReading)
                {
                    var geneExtension = gene.def.GetModExtension<GeneExtension>();
                    if (geneExtension == null ||
                        geneExtension.customBloodThingDef == null) continue;
                    return geneExtension.customBloodThingDef.graphicData.color;
                }
                return ColorUtils.GetDefaultBloodColor(pawn);
            } catch (Exception e)
            {
                VBLog.Warning($"Failed to generate color for {pawn.Label}: {e}");
                return Color.grey;
            }
        }
    }
}
