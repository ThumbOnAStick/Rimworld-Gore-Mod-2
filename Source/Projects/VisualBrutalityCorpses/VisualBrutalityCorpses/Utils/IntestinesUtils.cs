using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.Defs
{
    public static class IntestinesUtils
    {
        private static readonly string metaPath = "Intestines";

        private static Dictionary<BodyTypeDef, Texture2D> textures;

        private static readonly List<BodyTypeDef> supportedBodyTypes = new List<BodyTypeDef> {
            BodyTypeDefOf.Male,
            BodyTypeDefOf.Thin,
        };



        private static Dictionary<BodyTypeDef, Texture2D> LoadIntestinesTextures()
        {
            Dictionary<BodyTypeDef, Texture2D> result = new Dictionary<BodyTypeDef, Texture2D>();
            try
            {
                foreach (var bodyType in supportedBodyTypes)
                {
                    Texture2D item = ContentFinder<Texture2D>.Get(metaPath + "/" + bodyType.defName);
                    result.Add(bodyType, item);
                }
                return result;
            }catch(Exception e)
            {
                VBLog.ErrorSevere($"Intestines has zero valid textures! {e}");
                return result;
            }

        }

        public static void Init()
        {

            textures = LoadIntestinesTextures();
            if (textures.Values.Any(x => x.NullOrBad()))
            {
                VBLog.ErrorSevere($"Intestines has bad textures!");
            }

        }



        public static Texture2D GetIntestinesForBodyType(BodyTypeDef bodyType = null)
        {
            if (!textures.ContainsKey(bodyType) || textures[bodyType] == null)
                return GetIntestinesForBodyType(BodyTypeDefOf.Male);
            return textures[bodyType];
        }

    }
}
