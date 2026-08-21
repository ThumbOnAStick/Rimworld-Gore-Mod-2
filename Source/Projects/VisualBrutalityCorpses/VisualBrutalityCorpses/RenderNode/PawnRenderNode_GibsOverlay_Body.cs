using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Compatibility;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.RenderNode
{
    internal class PawnRenderNode_GibsOverlay_Body : PawnRenderNode
    {
        protected Apparel apperalCached;
        protected string graphicPathCached;
        protected readonly CompDeathRecorder deathRecorder;

        protected virtual string GibsOverlayPath => "VBOverlays/Gibs/Body/GibsOverlay";
        protected virtual string GraphicPath
        {
            get
            {
                if (!graphicPathCached.NullOrEmpty())
                {
                    return graphicPathCached;
                }

                if (this.tree.pawn.apparel.AnyApparel)
                {
                    BodyPartRecord corePart = this.tree.pawn.health.hediffSet.GetNotMissingParts().First(x => x.IsCorePart);
                    if (corePart == null) goto SkipApperalCheck;
                    IEnumerable<Apparel> apparels = this.tree.pawn.apparel.WornApparel.FindAll(x => x.def.apparel.CoversBodyPart(corePart));
                    if (apparels.EnumerableNullOrEmpty()) goto SkipApperalCheck;
                    Apparel apparel = apparels.OrderBy(x => x.def.apparel.LastLayer.drawOrder)?.Last();
                    if (apparel == null) goto SkipApperalCheck;
                    string path;
                    BodyTypeDef bodyType = tree.pawn.story.bodyType ?? BodyTypeDefOf.Male;
                    if (apparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead || apparel.def.apparel.LastLayer == ApparelLayerDefOf.EyeCover || apparel.RenderAsPack() || apparel.WornGraphicPath == BaseContent.PlaceholderImagePath || apparel.WornGraphicPath == BaseContent.PlaceholderGearImagePath)
                    {
                        path = apparel.WornGraphicPath;
                    }
                    else
                    {
                        path = apparel.WornGraphicPath + "_" + bodyType.defName;
                    }
                    apperalCached = apparel;
                    graphicPathCached = path;
                    return path;
                }
            SkipApperalCheck:
                if (this.tree.pawn == null || this.tree.pawn.story == null || this.tree.pawn.story.bodyType == null)
                {
                    return "";
                }
                graphicPathCached = this.tree.pawn.story.bodyType.bodyNakedGraphicPath;
                return this.tree.pawn.story.bodyType.bodyNakedGraphicPath;
            }

        }

        protected virtual Vector2 GetDrawSize(Pawn pawn)
        {

            if (apperalCached != null && apperalCached.def != null && apperalCached.def.graphicData != null)
            {
                return apperalCached.def.graphicData.drawSize;
            }
            return pawn.DrawSize;
        }




        public PawnRenderNode_GibsOverlay_Body(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
            : base(pawn, props, tree)
        {
            deathRecorder = pawn.TryGetComp<CompDeathRecorder>();
            graphicPathCached = "";
        }

        protected override void EnsureMeshesInitialized()
        {
            base.EnsureMeshesInitialized();
            graphicPathCached = "";
        }


        public override bool FlipGraphic(PawnDrawParms parms)
        {
            bool isFromWest = deathRecorder.IsGibsFromWest;
            if (parms.facing == Rot4.West || parms.facing == Rot4.East)
            {
                return base.FlipGraphic(parms);
            }
            return isFromWest;
        }

        public override Mesh GetMesh(PawnDrawParms parms)
        {
            var mesh = base.GetMesh(parms);
            return mesh;
        }


        public override Graphic GraphicFor(Pawn pawn)
        {
            if (deathRecorder == null) return null;
            if (GraphicPath.NullOrEmpty()) return null;
            return GraphicDatabase.Get<Graphic_Multi>(GibsOverlayPath, ShaderDatabase.CutoutSkinOverlay, drawSize: GetDrawSize(pawn), deathRecorder.GibsColor, Color.white, null, GraphicPath);
        }


    }
}
