using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;
using VisualBrutalityCorpses.Graphics;

namespace VisualBrutalityCorpses.Patches
{
    public class ApparelGraphicsPatch
    {
        public static void PatchHarmony()
        {
            MethodInfo original = AccessTools.Method(typeof(ApparelGraphicRecordGetter), "TryGetGraphicApparel");
            HarmonyMethod postfix = new HarmonyMethod(typeof(ApparelGraphicsPatch).GetMethod("Postfix"));
            MetaPatches.harmony.Patch(original, null, postfix);
        }

        static bool ValidateApparel(Apparel apparel)
        {
            if (apparel.Wearer == null) return false;
            if (!apparel.Wearer.Dead) return false;
            List<BodyPartGroupDef> defs = apparel.def.apparel.bodyPartGroups;
            bool validateParts = defs.Contains(BodyPartGroupDefOf.Torso) ||
                defs.Contains(BodyPartGroupDefOf.Legs);
            return validateParts;
        }

        public static void Postfix(Apparel apparel, BodyTypeDef bodyType, ref bool __result, ref ApparelGraphicRecord rec)
        {
            if (ModsConfig.IsActive("Thumb.ADV")) return;
            if (ValidateApparel(apparel) && rec.graphic != null)
            {
                var wrapper = new Graphic_MaskedSprite(rec.graphic, apparel.Wearer, apparel);
                if (wrapper.IsValid)
                {
                    rec.graphic = wrapper;
                }
            }
        }
    }
}
