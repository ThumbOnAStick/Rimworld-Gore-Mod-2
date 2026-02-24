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

        private bool Rotting
        {
            get
            {
                if (CompRottable == null) return false;
                return CompRottable.Stage == RotStage.Rotting;
            }
        }

        public override Color DrawColor { get => this.Rotting ? new Color(.1f, .5f, 0f) : base.DrawColor; set => base.DrawColor = value; }

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
            if (skullType == null) return null;
            return (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(skullType.graphicPath, ShaderDatabase.Cutout, Vector2.one, Color.white);
        }
        void DrawHead(Vector3 drawLoc)
        {
            if (headInfo == null) return;
            Graphic headGraphic = this.Graphic;
            if (headGraphic == null) return;
            Vector3 vector = drawLoc + new Vector3(0f, -0.01f, 0f);
            Quaternion rotation = this.Rotation.AsQuat;
            headGraphic.drawSize = this.headInfo.DrawSize * this.DrawSize;
            headGraphic.Draw(vector, this.headInfo.Facing, this, rotation.eulerAngles.y);
        }

        protected override void TickInterval(int delta)
        {
            base.TickInterval(delta);
            if (Rotting)
            {
                GasUtility.AddGas(Position, Map, GasType.RotStink, 1);
            }
        }

        void DrawHair(Vector3 drawLoc)
        {
            Vector3 hairDrawLoc = drawLoc + new Vector3(0f, 0f, 0f);
            Quaternion exactRotation = this.Rotation.AsQuat;
            HeadGraphics.DrawHairAndBeard(headInfo, this, hairDrawLoc, this.headInfo.DrawSize * this.DrawSize, exactRotation.eulerAngles.y);
        }
        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {

            // Head
            DrawHead(drawLoc);

            // Hair
            if (this.CompRottable?.Stage == RotStage.Dessicated) return;
            DrawHair(drawLoc);
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (var gizmos in base.GetGizmos())
            {
                yield return gizmos;

            }
            yield return new Command_Action()
            {
                defaultLabel = "VBTwist".Translate(),
                icon = TexUI.RotRightTex,
                action = () =>
                {
                    Rot4 rot = this.headInfo.Facing;
                    rot.Rotate(RotationDirection.Clockwise);
                    this.headInfo.Facing = rot;
                }
            };
        }


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref headInfo, "headInfo");
        }
    }
}
