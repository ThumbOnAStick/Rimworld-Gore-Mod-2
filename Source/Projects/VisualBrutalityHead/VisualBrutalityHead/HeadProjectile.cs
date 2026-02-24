using RimWorld;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using UnityEngine;
using VEF.Graphics;
using Verse;
using VisualBrutalityCorpses;
using VisualBrutalityCorpses.Utils;

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

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            try
            {
                if (VisualBrutalityMod.Settings.GenerateHeads &&
                    GenSpawn.TrySpawn(VBHeadDefOf.HeadItem, this.Position, this.MapHeld, out Thing thing))
                {
                    HeadItem headItem = (HeadItem)thing;
                    headItem.HeadInfoo = new HeadInfo(this.headInfo);
                    headItem.Rotation = Rot4.Random;
                    headItem.SetForbidden(true);
                }
            }
            catch (Exception ex)
            {
                VBLog.Error($"Failed to generate head. {ex}");
            }
            base.Destroy(mode);
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

        public override Vector2 DrawSize => headInfo.DrawSize * GetSizeMultiplier();

        public override Material DrawMat
        {
            get
            {
                return this.Graphic.MatSingleFor(this);
            }
        }


    }
}