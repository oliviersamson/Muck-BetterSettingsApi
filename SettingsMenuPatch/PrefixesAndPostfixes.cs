using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

namespace BetterSettingsApi.SettingsMenuPatch
{
    [HarmonyPatch]
    public class PrefixesAndPostfixes 
    {
        public static MethodBase TargetMethod()
        {

            return AccessTools.FirstMethod(AccessTools.TypeByName("MuckSettings.SettingsMenu"), method => method.Name.Contains("Prefix"));
        }

        [HarmonyPrefix]
        static bool PrefixPrefix(ref bool __result)
        {
            // Overwriting result not to skip original method
            __result = true;

            // Skip original method
            return false;
        }
    }
}
