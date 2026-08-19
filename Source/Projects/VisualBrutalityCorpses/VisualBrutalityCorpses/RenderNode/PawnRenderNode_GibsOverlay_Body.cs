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
        const string gibsOverlayPath = "VBOverlays/Gibs/Body/GibsOverlay";

        private readonly CompDeathRecorder deathRecorder;

        public PawnRenderNode_GibsOverlay_Body(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
            : base(pawn, props, tree)
        {
            deathRecorder = pawn.TryGetComp<CompDeathRecorder>();
        }
        
        public override Graphic GraphicFor(Pawn pawn)
        {
            if (deathRecorder == null) return null;
            string bodyNakedGraphicPath = pawn.story.bodyType.bodyNakedGraphicPath;
            if (bodyNakedGraphicPath == null) return null;
            return GraphicDatabase.Get<Graphic_Multi>(gibsOverlayPath, ShaderDatabase.CutoutSkinOverlay, drawSize: pawn.DrawSize, deathRecorder.GibsColor, Color.white, null, bodyNakedGraphicPath);

        }

    
    }
}
