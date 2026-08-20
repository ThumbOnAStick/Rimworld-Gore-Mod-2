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

    internal class PawnRenderNode_GibsOverlay_Head : PawnRenderNode_GibsOverlay_Body
    {

        protected override string GibsOverlayPath => "VBOverlays/Gibs/Head/GibsOverlay";
        protected override string GraphicPath
        {
            get
            {
                if (this.tree.pawn == null || this.tree.pawn.story == null || this.tree.pawn.story.bodyType == null)
                {
                    return "";
                }
                return this.tree.pawn.story.headType.graphicPath;
            }

        }

        public PawnRenderNode_GibsOverlay_Head(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
            : base(pawn, props, tree)
        {
        }
    }

}
    