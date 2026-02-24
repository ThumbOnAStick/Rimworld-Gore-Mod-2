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
            listing_Standard.CheckboxLabeled("DrawSkeleton".Translate(), ref settings.DrawSkeleton, tooltip: "DrawSkeleton.Explained".Translate());
            listing_Standard.CheckboxLabeled("GenerateFlesh".Translate(), ref settings.GenerateFlesh);
            listing_Standard.CheckboxLabeled("GenerateHeads".Translate(), ref settings.GenerateHeads);
            listing_Standard.End();
            settings.Write();
        }
    }
}
