using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterSettingsApi.OriginalSettingsPatch
{
    class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(Settings), "Start")]
        [HarmonyPostfix]
        static void StartPostfix(global::Settings __instance)
        {
            var nav = __instance.GetComponentInChildren<TopNavigate>();

            using (var page = new MuckSettings.Settings.Page(nav.settingMenus[0])) MuckSettings.Settings.Gameplay(page);
            using (var page = new MuckSettings.Settings.Page(nav.settingMenus[1])) MuckSettings.Settings.Controls(page);
            using (var page = new MuckSettings.Settings.Page(nav.settingMenus[2])) MuckSettings.Settings.Graphics(page);
            using (var page = new MuckSettings.Settings.Page(nav.settingMenus[3])) MuckSettings.Settings.Video(page);
            using (var page = new MuckSettings.Settings.Page(nav.settingMenus[4])) MuckSettings.Settings.Audio(page);
        }
    }
}
