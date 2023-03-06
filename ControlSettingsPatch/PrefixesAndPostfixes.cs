using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BetterSettingsApi.ControlSettingsPatch
{
    class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(MuckSettings.ControlSetting), "Awake")]
        [HarmonyPostfix]
        static void AwakePostfix(MuckSettings.ControlSetting __instance)
        {
            HorizontalLayoutGroup horizontalLayoutGroup = __instance.GetComponent<HorizontalLayoutGroup>();
            horizontalLayoutGroup.childForceExpandWidth = false;
            horizontalLayoutGroup.childAlignment = UnityEngine.TextAnchor.UpperLeft;

            LayoutRebuilder.ForceRebuildLayoutImmediate(horizontalLayoutGroup.GetComponent<RectTransform>());
            horizontalLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(horizontalLayoutGroup.preferredWidth, horizontalLayoutGroup.preferredHeight);
        }
    }
}
