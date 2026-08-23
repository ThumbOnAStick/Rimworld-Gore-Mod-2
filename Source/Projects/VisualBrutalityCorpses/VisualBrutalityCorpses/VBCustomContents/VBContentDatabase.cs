using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VEF.Weapons;
using Verse;
using VisualBrutalityCorpses.Compatibility;
using VisualBrutalityCorpses.Defs;
using VisualBrutalityCorpses.Utils;
            
namespace VisualBrutalityCorpses.VBCustomContents
{
    [StaticConstructorOnStartup]
    internal class VBContentDatabase
    {
        private static AssetBundle bundleInt;
        private static Dictionary<string, Shader> _lookupShaders;
        private const string NorthSuffix = "_north";
        private const string SouthSuffix = "_south";
        private const string EastSuffix = "_east";
        private static readonly string torsoDestroyedMasks = "VBMasks/TorsoDestroyedMasks";
        public static readonly Shader TestUnlitShader = LoadShader(Path.Combine("Assets", "testunlit.shader"));
        public static readonly Shader TestUnlitMixerShader = LoadShader(Path.Combine("Assets", "testunlitmixer.shader"));
        public static readonly List<Texture2D> TorsoDestroyedMasks = LoadAllTexturesFromFolder(torsoDestroyedMasks);
        public static readonly List<Texture2D> Skulls = LoadSkullTextures();
        //public static readonly Texture2D BurnedOverlay = LoadBurnedOverlay();

        static VBContentDatabase()
        {
            try
            {
                VBDefOf.CutMask.Init();
                VBDefOf.CrushMask.Init();
                VBDefOf.ShotMask.Init();
                IntestinesUtils.Init(); // Initialize Intestines
                // Initialize HAR 
                if (Compatibility_HAR.IsHARActive())
                {
                    Compatibility_HAR.AddCompInAlienDefs();
                }
            }
            catch (Exception e)
            {
                VBLog.ErrorSevere(e.Message);
            }
        }

        public static AssetBundle CBBundle
        {
            get
            {
                if (bundleInt == null)
                {
                    bundleInt = VisualBrutalityMod.Instance.MainBundle;
                }
                return bundleInt;
            }
        }

        public static Texture2D GetSplitInHalfMask()
        {
            return TorsoDestroyedMasks[0];
        }


        private static List<Texture2D> LoadAllTexturesFromFolder(string folderPath)
        {
            List<Texture2D> result = new List<Texture2D>();
            IEnumerable<Texture2D> textures = ContentFinder<Texture2D>.GetAllInFolder(folderPath);
            if (textures != null)
            {
                result.AddRange(textures);
            }
            return result;
        }

        private static List<Texture2D> LoadSkullTextures()
        {
            string path = HeadTypeDefOf.Skull.graphicPath;
            List<Texture2D> result = new List<Texture2D>
            {
                ContentFinder<Texture2D>.Get(path + SouthSuffix),
                ContentFinder<Texture2D>.Get(path + NorthSuffix),
                ContentFinder<Texture2D>.Get(path + EastSuffix),
            };
            return result;
        }

        public static Texture2D GetSkullTexture(Rot4 rot)
        {
            if(rot == Rot4.South)
            {
                return Skulls[0];
            }
            if(rot == Rot4.North)
            {
                return Skulls[1];
            }
            return Skulls[2];
        }

        private static Shader LoadShader(string shaderName)
        {
            if (_lookupShaders == null) _lookupShaders = new Dictionary<string, Shader>();

            if (!_lookupShaders.ContainsKey(shaderName))
            {
                _lookupShaders[shaderName] = CBBundle.LoadAsset<Shader>(shaderName);
            }
            Shader shader = _lookupShaders[shaderName];

            if (shader != null) return shader;
            VBLog.ErrorSevere("Could not load shader: " + shaderName);
            return ShaderDatabase.DefaultShader;
        }
    }
}
