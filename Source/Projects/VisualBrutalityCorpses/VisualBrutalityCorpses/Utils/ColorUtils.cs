using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Compatibility;

namespace VisualBrutalityCorpses.Utils
{
    public static class ColorUtils
    {

        public static Color GetDefaultBloodColor(Pawn pawn)
        {
            if (pawn.def.race == null || 
                pawn.def.race.BloodDef == null || 
                pawn.def.race.BloodDef.graphicData == null || 
                pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Dessicated) return Color.grey;
            Color result = pawn.def.race.BloodDef.graphicData.color;
            if (ModsConfig.AnomalyActive && pawn.IsShambler)
            {
                float factor = .25f;
                result = new Color(result.r * factor, result.g * factor, result.b * factor, 1);
            }
            return result;
        }

        public static Color GetSkeletonColor(Pawn pawn)
        {
            var result = GetBloodColor(pawn);
            return result * 2f;
        }

        public static Color GetBloodColor(Pawn pawn)
        {
            if (pawn.def.race == null || pawn.def.race.BloodDef == null || pawn.def.race.BloodDef.graphicData == null) return Color.grey;

            if (ModsConfig.IsActive("OskarPotocki.VanillaFactionsExpanded.Core") && ModsConfig.BiotechActive)
            {
                return Compatibility_VEF.GetPawnBloodColor(pawn);
            }

            return GetDefaultBloodColor(pawn);
        }
    }
}
