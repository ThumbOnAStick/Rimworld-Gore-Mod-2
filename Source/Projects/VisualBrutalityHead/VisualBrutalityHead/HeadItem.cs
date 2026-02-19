using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace VisualBrutalityHead
{
    public class HeadItem : ThingWithComps
    { 
        private HeadInfo headInfo;

        public HeadInfo HeadInfoo
        {
            get => headInfo; set => headInfo = value;
        }
        public HeadItem()
        {
            
        }

        public HeadItem(HeadInfo headInfo)
        {
            this.headInfo = new HeadInfo(headInfo);
        }

        public override string Label => "VBHead".Translate(headInfo?.PawnName);
        public CompRottable CompRottable => this.TryGetComp<CompRottable>();


        public override Graphic Graphic
        {
            get
            {
                return this.CompRottable?.Stage == RotStage.Dessicated ? GetSkullGraphic() : HeadGraphics.GetFlyingHeadGraphic(this.headInfo);
            }
        }
        private Graphic GetSkullGraphic()
        {
            var skullType = HeadTypeDefOf.Skull;
            Graphic_Multi graphic_Multi = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(skullType.graphicPath, ShaderDatabase.Cutout, Vector2.one, Color.white);
            return graphic_Multi;
        }
        void DrawHead(Vector3 drawLoc)
        {
            Graphic headGraphic = this.Graphic;
            if (headGraphic == null)
            {
                return;
            }

            Vector3 vector = drawLoc + new Vector3(0f, -0.01f, 0f);
            Quaternion rotation = this.Rotation.AsQuat;
            headGraphic.drawSize = this.DrawSize;
            headGraphic.Draw(vector, this.headInfo.Facing, this, rotation.eulerAngles.y);
        }

        void DrawHair(Vector3 drawLoc)
        {
            Vector3 hairDrawLoc = drawLoc + new Vector3(0f, 0f, 0f);
            Quaternion exactRotation = this.Rotation.AsQuat;
            HeadGraphics.DrawHairAndBeard(headInfo, this, hairDrawLoc, this.DrawSize, exactRotation.eulerAngles.y);
        }
        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {

            // Head
            DrawHead(drawLoc);

            // Hair
            if (this.CompRottable?.Stage == RotStage.Dessicated) return;
            DrawHair(drawLoc);
        }


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref headInfo, "headInfo");
        }
    }
}
