using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MuckSettings;
using System.Reflection;
using UnityEngine.UI;

namespace BetterSettingsApi.UpdateKeyListenerPatch
{
    [HarmonyPatch]
    class UpdateKeyListener_Prefix
    {
        public static MethodBase TargetMethod()
        {

            return AccessTools.FirstMethod(AccessTools.TypeByName("MuckSettings.UpdateKeyListener"), method => method.Name.Contains("Prefix"));
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            CodeMatcher codeMatcher = new(instructions);

            // Go to SetKey call
            codeMatcher = codeMatcher.MatchForward(false, new CodeMatch(OpCodes.Ldsfld));
            codeMatcher = codeMatcher.MatchForward(false, new CodeMatch(OpCodes.Callvirt));

            // Get the KeyListener __instance on top of the stack
            codeMatcher = codeMatcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0));

            // Replace instruction with call to delgate, consuming both KeyListenerCurrentlyChanging.value, the keycode and the KeyListener instance
            codeMatcher = codeMatcher.SetInstructionAndAdvance(Transpilers.EmitDelegate<Action<MuckSettings.ControlSetting, KeyCode, KeyListener>>(
                (controlSetting, keyCode, keyListener) => {

                    if (controlSetting == null)
                    {
                        keyListener.currentlyChanging.SetKey(keyCode);
                        Plugin.Log.LogDebug("Setting original controlSetting key");
                    }
                    else
                    {
                        controlSetting.SetKey(keyCode);
                        Plugin.Log.LogDebug("Setting MuckSettings' controlSetting key");
                    }
                }));

            return codeMatcher.InstructionEnumeration();
        }
    }
}
