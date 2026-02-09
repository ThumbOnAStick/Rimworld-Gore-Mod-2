using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Utils;
namespace VisualBrutalityCorpses
{
    public class VisualBrutalityMod : Mod
    {
        public static VisualBrutalityMod Instance { get; private set; }

        private readonly VBSettings settings;

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
    }
}
