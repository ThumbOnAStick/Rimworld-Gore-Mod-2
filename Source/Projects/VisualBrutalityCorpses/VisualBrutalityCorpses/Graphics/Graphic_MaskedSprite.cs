using MVCF.Features;
using RimWorld;
using RimWorld.BaseGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VEF.Weapons;
using Verse;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Utils;
using VisualBrutalityCorpses.VBCustomContents;

namespace VisualBrutalityCorpses.Graphics
{
    public class Graphic_MaskedSprite : Graphic
    {
        protected readonly Graphic inner;
        protected readonly Thing targetThing;
        protected readonly bool isBody;
        private readonly Dictionary<Rot4, MaskedSpriteRotDrawer> drawers = new Dictionary<Rot4, MaskedSpriteRotDrawer>();
        public bool IsValid => inner != null && targetThing != null;

        public Graphic_MaskedSprite(Graphic inner, Pawn pawn, Apparel apparel = null, bool isBody = true)
        {
            this.inner = inner;
            if (apparel == null)
                this.targetThing = pawn;
            else
                this.targetThing = apparel;
            if (inner == null || targetThing == null)
            {
                return;
            }

            this.data = inner.data;
            this.color = inner.color;
            this.colorTwo = inner.colorTwo;
            this.isBody = isBody;
            this.drawSize = inner.drawSize;
        }

        public override string ToString() => $"Graphic_MaskedSprite({inner})";

        public override Material MatSingle => inner?.MatSingle;

        protected Material EvaluateBurntMaterials(Material baseMat,
            Rot4 rot,
            Thing thing)
        {
            if (thing is Apparel apparel1)
            {
                if (!drawers.TryGetValue(rot, out MaskedSpriteRotDrawer drawer))
                {
                    drawers[rot] = drawer = new MaskedSpriteRotDrawer();
                }
                var mat = drawer.GetMaterial(baseMat, VBContentDatabase.GetSplitInHalfMask(), apparel1.Wearer, apparel1);
                mat.color = new Color(.1f, .1f, .1f);
                return mat;
            }

            var burnedMat = new Material(baseMat)
            {
                color = new Color(.1f, .1f, .1f)
            };
            return burnedMat;
        }

        public override Material MatAt(Rot4 rot, Thing thing = null)
        {
            if (inner == null)
            {
                return base.MatAt(rot, thing);
            }

            try
            {
                var baseMat = inner.MatAt(rot, thing);

                if (targetThing == null)
                {
                    return baseMat;
                }

                var recorder = targetThing.TryGetComp<CompDeathRecorder>();
                if (targetThing is Apparel apparel)
                    recorder = apparel.Wearer.TryGetComp<CompDeathRecorder>();
                if (recorder == null || !recorder.HasSpecialCorpseMask)
                {
                    return baseMat;
                }
                if (recorder.Burnt) return EvaluateBurntMaterials(baseMat, rot, targetThing);

                    Texture2D texture;
                bool isAnimalOrEntity = (thing != null && !thing.def.race.Humanlike) || 
                                (thing is Corpse corpse && !corpse.def.race.Humanlike) ||
                                (targetThing is Pawn pawnTarget && !pawnTarget.def.race.Humanlike);
                if (isAnimalOrEntity)
                {
                    texture = recorder.GetGoreMaskAnimal;
                }
                else
                {
                    texture = isBody? recorder.GetGoreTextureBody : recorder.GetGoreTextureHead;
                }
                if (texture == null)
                {
                    VBLog.Error("Generated a null texture!!!");
                    return baseMat;
                }

                if (!drawers.TryGetValue(rot, out MaskedSpriteRotDrawer drawer))
                {
                    drawers[rot] = drawer = new MaskedSpriteRotDrawer();
                }

                if (targetThing is Apparel apparel1)
                    return drawer.GetMaterial(baseMat, texture, apparel1.Wearer, apparel1);
                return drawer.GetMaterial(baseMat, texture, (Pawn)targetThing, null, isBody);

            }
            catch (Exception e)
            {
                string targetName = targetThing != null ? targetThing.ThingID : "None";
                VBLog.Error($"Failed to draw maksed sprite mat for {targetName}, stacktrace: {e}");
                return inner.MatAt(rot, thing);
            }
        }

        public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
        {
            inner?.DrawWorker(loc, rot, thingDef, thing, extraRotation);
        }

    }
}
