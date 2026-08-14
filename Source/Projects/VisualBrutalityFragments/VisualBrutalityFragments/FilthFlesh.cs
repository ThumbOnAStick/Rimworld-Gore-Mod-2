using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityFragments
{
    public class FilthFlesh : ThingWithComps
    {
        private bool isAnomaly;
        private Color overrideColor = Color.white;

        public void SetOverrideColor(Pawn pawn)
        {
            if (pawn == null)
            {
                return;
            }
            overrideColor = ColorUtils.GetBloodColor(pawn);
        }

        public void SetOverrideColor(Color color)
        {
            this.SetColor(color);
            this.overrideColor = color;
        }
        public void SetIsAnomaly(bool isAnomaly)
        {
            this.isAnomaly = isAnomaly;
        }

        
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<Color>(ref overrideColor, "overrideColor"); 
            Scribe_Values.Look<bool>(ref isAnomaly, "isAnomaly");
        }
    }
}
