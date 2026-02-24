using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace VisualBrutalityCorpses.Utils
{
    public static class VBLog
    {
        static string Tag => "["+ "VBModTitle".Translate() + "] ";
        public static void Message(string message)
        {
            Log.Message(Tag + message);
        }

        public static void Warning(string message)
        {
            Log.Warning(Tag + message);
        }

        public static void Error(string message)
        {
            Log.Error(Tag + message);
        }

        public static void ErrorSevere(string message)
        {
            Error(message);
            //TaggedString fullMsg = (Tag + "VBErrorSevere".Translate(message)).Colorize(Color.red);
            //Find.WindowStack.Add(new Dialog_MessageBox(fullMsg));
        }
    }
}
