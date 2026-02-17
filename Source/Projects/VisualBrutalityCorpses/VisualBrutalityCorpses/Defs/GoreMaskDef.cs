using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace VisualBrutalityCorpses.Defs
{
    public class GoreMaskDef : Def
    {

        public string metaPath;

        public List<BodyTypeDef> supportedBodyTypes;

        private static readonly List<string> facingPaths = new List<string>() { "South", "North", "East" };
     
        private List<Texture2D> headTextures;

        private Dictionary<BodyTypeDef, List<Texture2D>> bodyTextureDict;

        public bool HasHeadTex
        {
            get
            {
                return !this.headTextures.NullOrEmpty();
            }
        }

        public bool HasBodyTex
        {
            get
            {
                return !this.bodyTextureDict.NullOrEmpty();
            }
        }

        public void Init()
        {
            headTextures = LoadHeadTextures();

            bodyTextureDict = LoadBodyTextures();
        }

        private List<Texture2D> LoadDirections(string path)
        {
            List<Texture2D> result = new List<Texture2D>();
            IEnumerable<Texture2D> folderContents = ContentFinder<Texture2D>.GetAllInFolder(path);
            if (folderContents.EnumerableNullOrEmpty())
            {
                return result;
            }
            foreach (var facing in facingPaths)
            {
                result.Add(ContentFinder<Texture2D>.Get(path + facing));
            }
            return result;
        }

        private List<Texture2D> LoadHeadTextures()
        {
            return LoadDirections(metaPath + "/Head/");
        }

        private Dictionary<BodyTypeDef, List<Texture2D>> LoadBodyTextures()
        {
            Dictionary<BodyTypeDef, List<Texture2D>> result = new Dictionary<BodyTypeDef, List<Texture2D>>();
            foreach (var bodyType in supportedBodyTypes)
            {
                List<Texture2D> item = LoadDirections(metaPath + "/Body/" + bodyType.defName + "/");
                result.Add(bodyType, item);
            }
            return result;
            
        }

        public Texture2D GetBodyMaskInRot(BodyTypeDef bodyType, Rot4 rot)
        {
            if (!bodyTextureDict.ContainsKey(bodyType) || bodyTextureDict[bodyType].Count <= 2) return GetBodyMaskInRot(BodyTypeDefOf.Male, rot);
            if (rot == Rot4.North)
                return bodyTextureDict[bodyType][1];
            if (rot == Rot4.East || rot == Rot4.West)
                return bodyTextureDict[bodyType][2];

            return bodyTextureDict[bodyType][0];
        }

        public Texture2D GetHeadMaskInRot(Rot4 rot)
        {
            if (headTextures.Count <= 2) return null;
            if (rot == Rot4.North)
                return headTextures[1];
            if (rot == Rot4.East || rot == Rot4.West)
                return headTextures[2];

            return headTextures[0];
        }
    }
}
