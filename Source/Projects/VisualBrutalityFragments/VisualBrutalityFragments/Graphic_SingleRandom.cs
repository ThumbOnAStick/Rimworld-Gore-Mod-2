using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityFragments
{
    public class Graphic_SingleRandom : Graphic_Single
    {

        public override void Init(GraphicRequest req)
        {
            VBLog.Message("Graphic single random init");
            data = req.graphicData;
            path = req.path;
            maskPath = req.maskPath;
            color = req.color;
            colorTwo = req.colorTwo;
            drawSize = req.drawSize;
            var allTextures = ContentFinder<Texture2D>.GetAllInFolder(req.path);
            MaterialRequest materialRequest = new MaterialRequest(allTextures.RandomElement(), req.shader, color)
            {
                colorTwo = colorTwo,
                renderQueue = req.renderQueue,
                shaderParameters = req.shaderParameters
            };
            MaterialRequest req2 = materialRequest;
            if (req.shader.SupportsMaskTex())
            {
                req2.maskTex = ContentFinder<Texture2D>.Get(maskPath.NullOrEmpty() ? (path + MaskSuffix) : maskPath, reportFailure: false);
            }

            mat = MaterialPool.MatFrom(req2);
        }
    }
}
