using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Utils;
namespace VisualBrutalityCorpses
{
    public class VisualBrutalityMod : Mod
    {
        private readonly VBSettings settings;

        public static VisualBrutalityMod Instance { get; private set; }

        public AssetBundle MainBundle{
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

                string bundlePath = Path.Combine(base.Content.RootDir, "Materials\\Bundles\\" + text + "\\testunlit");
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
                listing_Standard.CheckboxLabeled("VBDrawIntestines".Translate(), ref settings.DrawIntestines);
            }
            if (listing_Standard.ButtonText("VBSettingsDefault".Translate()))
            {
                settings.Restore();
            }
            listing_Standard.End();
            settings.Write();
        }
    }
}
