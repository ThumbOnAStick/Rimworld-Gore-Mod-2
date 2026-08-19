using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualBrutalityCorpses.Utils;

namespace VisualBrutalityCorpses.Compatibility
{
    internal static class BodytypeCheck
    {
        private static readonly List<String> bodies = new List<String>() {"PMP_PersonaMech"};
        public static bool IsSupported(string bodyType)
        {
            bool result = !bodies.Contains(bodyType);
            return result;
        }
    }
}
