using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSettingsApi.SettingsPatch
{
    class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(MuckSettings.Settings), "Gameplay")]
        [HarmonyPrefix]
        static bool GamePlayPrefix()
        {
            // Skip original method
            return false;
        }

        [HarmonyPatch(typeof(MuckSettings.Settings), "Controls")]
        [HarmonyPrefix]
        static bool ControlsPrefix()
        {
            // Skip original method
            return false;
        }

        [HarmonyPatch(typeof(MuckSettings.Settings), "Graphics")]
        [HarmonyPrefix]
        static bool GraphicsPrefix()
        {
            // Skip original method
            return false;
        }

        [HarmonyPatch(typeof(MuckSettings.Settings), "Video")]
        [HarmonyPrefix]
        static bool VideoPrefix()
        {
            // Skip original method
            return false;
        }

        [HarmonyPatch(typeof(MuckSettings.Settings), "Audio")]
        [HarmonyPrefix]
        static bool AudioPrefix()
        {
            // Skip original method
            return false;
        }
    }
}
