using AlienRace;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Graphics;
using VisualBrutalityCorpses.Patches;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.Compatibility
{
    public static class Compatibility_HAR
    {
        public static bool IsHARActive()
        {
            if (!ModsConfig.IsActive("erdelf.HumanoidAlienRaces")) return false;
            return true;
        }

        public static bool IsPawnAlien(Pawn pawn)
        {
            if (pawn?.def == null) return false;
            return pawn.def.defName.ToLower() != "human" && pawn.def.GetType().Name == "ThingDef_AlienRace";
        }

        public static void ApplyHARBodyPrefix(ref Graphic g, PawnRenderNode_Body instance, Pawn pawn)
        {
            AlienRenderTreePatches.BodyGraphicForPrefix(instance, pawn, ref g);
        }

        public static void ApplyHARHeadPrefix(ref Graphic g, PawnRenderNode_Head instance, Pawn pawn)
        {
            AlienRenderTreePatches.HeadGraphicForPrefix(instance, pawn, ref g);
        }

        public static bool HasHeadGraphics(Pawn pawn, out string path, out Vector2 drawSize)
        {
            path = null;
            drawSize = Vector2.one;
            if (!IsPawnAlien(pawn)) return false;

            ThingDef_AlienRace alienRaceDef = pawn.def as ThingDef_AlienRace;
            if (alienRaceDef?.alienRace?.graphicPaths?.head == null) return false;

            var gen = alienRaceDef.alienRace.generalSettings.alienPartGenerator;
            var headDrawSize = gen.customHeadDrawSize;
            drawSize = headDrawSize != Vector2.one ? headDrawSize : gen.customDrawSize;
            int savedIndex = pawn.HashOffset();
            path = alienRaceDef.alienRace.graphicPaths.head.GetPath(pawn, ref savedIndex);
            return path != null;
        }


        public static void AddCompInAlienDefs()
        {
            var alienDefs = DefDatabase<ThingDef_AlienRace>.AllDefs;
            foreach (var alienDef in alienDefs)
            {
                if(alienDef.alienRace.graphicPaths.body != null)
                {
                    AddCompInAlienDef(alienDef);
                }
            }
        }

        static void AddCompInAlienDef(ThingDef alienDef)
        {
            if (alienDef == null) return;
            if (alienDef.comps == null)
            {
                alienDef.comps = new System.Collections.Generic.List<CompProperties>();
            }
            if (alienDef.comps.Any(x => x is CompProperties_DeathRecorder)) return;
            alienDef.comps.Add(new CompProperties_DeathRecorder());
        }
    }
}
