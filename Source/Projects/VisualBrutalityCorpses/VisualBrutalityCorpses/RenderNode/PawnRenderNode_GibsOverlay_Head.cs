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

    internal class PawnRenderNode_GibsOverlay_Head : PawnRenderNode
    {
        const string gibsOverlayPath = "VBOverlays/Gibs/Head/GibsOverlay";

        public PawnRenderNode_GibsOverlay_Head(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
            : base(pawn, props, tree)
        {
        }

        public override Graphic GraphicFor(Pawn pawn)
        {
            var deathRecorder = pawn.TryGetComp<CompDeathRecorder>();
            if (deathRecorder == null) return null;   
            string headGraphicPath = pawn.story.headType.graphicPath;
            if (headGraphicPath == null) return null;
            if (!deathRecorder.IsGibSpilled)
            {
                return null;
            }
            return GraphicDatabase.Get<Graphic_Multi>(gibsOverlayPath, ShaderDatabase.CutoutSkinOverlay, drawSize: pawn.DrawSize, deathRecorder.GibsColor, Color.white, null, headGraphicPath);

        }
    }

}
