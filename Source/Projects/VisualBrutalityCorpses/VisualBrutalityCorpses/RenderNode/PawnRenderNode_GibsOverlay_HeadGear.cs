using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Comps;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.RenderNode
{

    internal class PawnRenderNode_GibsOverlay_HeadGear : PawnRenderNode_GibsOverlay_Body
    {
        protected override string GraphicPath
        {
            get
            {
                if (!graphicPathCached.NullOrEmpty())
                {
                    return graphicPathCached;
                }

                if (this.tree.pawn.apparel.AnyApparel)
                {
                    var hasHead = this.tree.pawn.health.hediffSet.TryGetBodyPartRecord(BodyPartDefOf.Head, out BodyPartRecord headPart);
                    if (!hasHead) goto SkipApperalCheck;
                    IEnumerable<Apparel> apparels = this.tree.pawn.apparel.WornApparel.FindAll(x => x.def.apparel.CoversBodyPart(headPart));
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
                return "";
            }

        }
        protected override string GibsOverlayPath => "VBOverlays/Gibs/Head/GibsOverlay";


        public PawnRenderNode_GibsOverlay_HeadGear(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
            : base(pawn, props, tree)
        {
        }
    }

}
    