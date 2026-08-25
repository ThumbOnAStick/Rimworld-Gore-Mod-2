using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Graphics;
using VisualBrutalityCorpses.Utils;
using VisualBrutalityCorpses.VBCustomContents;
using static Verse.ArenaUtility.ArenaResult;

namespace VisualBrutalityCorpses.RenderNode
{
    internal class PawnRenderSubWorker_Various : PawnRenderSubWorker
    {

        private readonly Dictionary<Rot4, MaskedSpriteRotDrawer> drawers = new Dictionary<Rot4, MaskedSpriteRotDrawer>();


        /// <summary>
        /// When pawn is burnt, return burnt material
        /// </summary>
        /// <param name="baseMat">The original material</param>
        /// <param name="rot">Rot4 rotation</param>
        /// <param name="thing">Target thing</param>
        /// <returns></returns>
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

        public override void EditMaterial(PawnRenderNode node, PawnDrawParms parms, ref Material material)
        {
            try
            {
                var pawn = node.tree.pawn;

                if (pawn == null || !pawn.Dead)
                {
                    return;
                }

                var recorder = pawn.TryGetComp<CompDeathRecorder>();

                if (recorder == null)
                {
                    return;
                }
                if (recorder.Burnt)
                {
                    material = EvaluateBurntMaterials(material, parms.facing, pawn);
                    return;
                }

                if (!recorder.HasSpecialCorpseMask)
                {
                    return;
                }

                Texture2D texture;
                bool isAnimalOrEntity = !pawn.def.race.Humanlike;
                bool isBody = node is PawnRenderNode_Body || (node is PawnRenderNode_Apparel && node.apparel!=null && node.apparel.def.apparel.CoversBodyPartGroup(BodyPartGroupDefOf.Torso));
                if (isAnimalOrEntity)
                {
                    texture = recorder.GetGoreMaskAnimal;
                }
                else
                {
                    texture = isBody ? recorder.GetGoreTextureBody : recorder.GetGoreTextureHead;
                }
                if (texture == null)
                {
                    VBLog.Error("Generated a null texture!!!");
                    return;
                }

                if (!drawers.TryGetValue(parms.facing, out MaskedSpriteRotDrawer drawer))
                {
                    drawers[parms.facing] = drawer = new MaskedSpriteRotDrawer();
                }

                if (node is PawnRenderNode_Apparel apparelNode && apparelNode.apparel != null)
                {
                    material = drawer.GetMaterial(material, texture, pawn, apparelNode.apparel);
                    return;
                }
                material = drawer.GetMaterial(material, texture, pawn, null, isBody);
            }
            catch (Exception e)
            {
                string targetName = node.tree.pawn != null ? node.tree.pawn.ThingID : "None";
                VBLog.Error($"Failed to draw maksed sprite mat for {targetName} on {node.GetType()}, stacktrace: {e}");
            }
        }

    }
}
