using RimWorld;
using Verse;

namespace VisualBrutalityCorpses.Graphics
{
    public static class MaskedSpriteHelper
    {
        public static Graphic CreateBodySprite(Graphic inner, Pawn pawn)
            => VisualBrutalityMod.Settings.OverrideCorpseTexture? new Graphic_MaskedSprite(inner, pawn) : inner;

        public static Graphic CreateHeadSprite(Graphic inner, Pawn pawn)
            => VisualBrutalityMod.Settings.OverrideCorpseTexture ? new Graphic_MaskedSprite(inner, pawn, null, false): inner;

        public static Graphic_MaskedSprite CreateApparelSprite(Graphic inner, Pawn pawn, Apparel apparel)
            => new Graphic_MaskedSprite(inner, pawn, apparel);
    }
}
