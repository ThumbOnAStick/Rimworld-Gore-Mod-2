using RimWorld;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using UnityEngine;
using VEF.Graphics;
using Verse;

namespace VisualBrutalityHead
{
    public class HeadProjectile : Projectile
    {
        private HeadInfo headInfo;
        private int frame;
        private readonly float refreshTick = 10;


        public HeadInfo HeadInfoo
        {
            get => headInfo;
            set => headInfo = value;
        }

        public HeadProjectile() : base()
        {
           this.headInfo = new HeadInfo();
        }
        public override Graphic Graphic
        {
            get
            {
                return HeadGraphics.GetFlyingHeadGraphic(this.headInfo);
            }
        }


        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            HeadItem projectile_FlyingHead = (HeadItem)GenSpawn.Spawn(VBHeadDefOf.HeadItem, this.Position, this.MapHeld, WipeMode.Vanish);
            projectile_FlyingHead.HeadInfoo = new HeadInfo(this.headInfo);
            projectile_FlyingHead.Rotation = Rot4.Random;
            base.Impact(hitThing, blockedByShield);
        }


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref headInfo, "headInfo");
        }

        protected override void TickInterval(int delta)
        {
            base.TickInterval(delta);
            this.frame++;
            this.headInfo.Facing = Rot4.FromAngleFlat(360 * ((frame % refreshTick) / refreshTick));
        }

        void DrawHead(Vector3 drawLoc)
        {
            Graphic headGraphic = this.Graphic;
            if (headGraphic == null)
            {
                return;  
            }

            Vector3 vector = drawLoc + new Vector3(0f, -0.01f, 0);
            Quaternion rotation = ExactRotation;
            headGraphic.drawSize = this.DrawSize;
            headGraphic.Draw(vector, this.headInfo.Facing, this, rotation.eulerAngles.y);
        }

        void DrawHair(Vector3 drawLoc)
        {
            Vector3 hairDrawLoc = drawLoc + new Vector3(0f, 0f, 0);
            Quaternion exactRotation = this.ExactRotation;
            HeadGraphics.DrawHairAndBeard(headInfo, this, hairDrawLoc, this.DrawSize, exactRotation.eulerAngles.y);
        }
        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {

            // Head
            DrawHead(drawLoc);

            // Hair
            DrawHair(drawLoc);
        }

        public override Quaternion ExactRotation => base.ExactRotation * Quaternion.Euler(0, 90, 0);

 
        float GetSizeMultiplier()
        {
            float distanceFrom05 = Mathf.Abs(0.5f - this.DistanceCoveredFraction);
            return Mathf.Max(1, 2f - (distanceFrom05 * 2f));
        }

        public override Vector2 DrawSize => headInfo.Pawn != null ? headInfo.Pawn.DrawSize * GetSizeMultiplier() : Vector2.one * GetSizeMultiplier();

        public override Material DrawMat
        {
            get
            {
                return this.Graphic.MatSingleFor(this);
            }
        }


    }
}