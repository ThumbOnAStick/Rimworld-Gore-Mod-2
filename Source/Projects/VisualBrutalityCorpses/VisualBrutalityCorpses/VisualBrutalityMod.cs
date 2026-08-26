using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Windows;
using Verse;
using VisualBrutalityCorpses.Utils;
namespace VisualBrutalityCorpses
{
    public class VisualBrutalityMod : Mod
    {
        const string packageName = "visualbrutality";

        private readonly VBSettings settings;

        public static VisualBrutalityMod Instance { get; private set; }

        public AssetBundle MainBundle
        {
            get
            {
                string text = "";

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    text = "StandaloneOSX";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    text = "StandaloneWindows64";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    text = "StandaloneLinux64";
                }

                string bundlePath = Path.Combine(base.Content.RootDir, "Materials\\Bundles\\" + text + $"\\{packageName}");
                AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);

                if (bundle == null)
                {
                    VBLog.Error("Failed to load bundle at path: " + bundlePath);
                }
                return bundle;
            }
        }

        public static VBSettings Settings =>
            Instance != null
                ? Instance.settings
                : LoadedModManager.GetMod<VisualBrutalityMod>().GetSettings<VBSettings>();
        public VisualBrutalityMod(ModContentPack content) : base(content)
        {
            Instance = this;
            settings = GetSettings<VBSettings>();
        }

        public override string SettingsCategory()
        {
            return "VBModTitle".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.CheckboxLabeled("VBGenerateFlesh".Translate(), ref settings.GenerateFlesh);
            if (settings.GenerateFlesh)
            {
                listing_Standard.CheckboxLabeled("VBFilthMode".Translate(), ref settings.FilthMode, tooltip: "VBFilthMode.Explained".Translate());
            }

            listing_Standard.GapLine();
            listing_Standard.CheckboxLabeled("VBGenerateHeads".Translate(), ref settings.GenerateHeads);

            listing_Standard.GapLine();
            listing_Standard.CheckboxLabeled("VBOverrideCorpseTexture".Translate(), ref settings.OverrideCorpseTexture);
            if (settings.OverrideCorpseTexture)
            {
                listing_Standard.CheckboxLabeled("VBDrawSkeleton".Translate(), ref settings.DrawSkeleton, tooltip: "VBDrawSkeleton.Explained".Translate());
                //listing_Standard.CheckboxLabeled("VBDrawIntestines".Translate(), ref settings.DrawIntestines, tooltip: "VBDrawIntestines.Explained".Translate());
                float raw = listing_Standard.SliderLabeled("VBTorsoSplitThreshold".Translate(settings.TorsoSplitThreshold * 100), settings.TorsoSplitThreshold, 0.5f, 2.0f, tooltip: "VBTorsoSplitThreshold.Explained".Translate());
                settings.TorsoSplitThreshold = (float)(Math.Round(raw * 4f, MidpointRounding.AwayFromZero) / 4f);
                
            }
            listing_Standard.GapLine();

            listing_Standard.CheckboxLabeled("VBEnableGibsOverlay".Translate(), ref settings.EnableGibsOverlay, tooltip: "VBEnableGibsOverlay.Explained".Translate());
            if (settings.EnableGibsOverlay)
                settings.GibsOverlayDuration = (int)listing_Standard.SliderLabeled("VBGibsOverlayDuration".Translate(settings.GibsOverlayDuration), settings.GibsOverlayDuration, 1, 1000, tooltip: "VBGibsOverlayDuration.Explained".Translate());
            listing_Standard.GapLine();

            if (listing_Standard.ButtonText("VBSettingsDefault".Translate()))
            {
                settings.Restore();
            }
            listing_Standard.End();
            settings.Write();
        }
    }
}
