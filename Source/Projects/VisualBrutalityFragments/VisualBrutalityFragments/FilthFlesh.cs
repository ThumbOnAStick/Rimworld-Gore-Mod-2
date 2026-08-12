using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace VisualBrutalityFragments
{
    public class FilthFlesh : ThingWithComps
    {
        private ThingDef fleshDef;
        private bool isAnomaly;

        public void SetFleshDef(ThingDef fleshDef)
        {
            this.fleshDef = fleshDef;
        }
        public void SetIsAnomaly(bool isAnomaly)
        {
            this.isAnomaly = isAnomaly;
        }


        public override Graphic Graphic
        {
            get
            {
                var graphic = this.fleshDef != null ? fleshDef.graphic : base.Graphic;
                
                return isAnomaly?graphic.GetColoredVersion(ShaderDatabase.Cutout, new Color(.3f,0,0), Color.white): graphic;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<ThingDef>(ref fleshDef, "fleshDef"); 
            Scribe_Values.Look<bool>(ref isAnomaly, "isAnomaly");
        }
    }
}
