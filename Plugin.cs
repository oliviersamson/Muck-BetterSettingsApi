using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace BetterSettingsApi
{
    public static class Globals
    {
        public const string PLUGIN_GUID = "muck.mrboxxy.bettersettingsapi";
        public const string PLUGIN_NAME = "BetterSettingsApi";
        public const string PLUGIN_VERSION = "1.0.1";
    }

    [BepInPlugin(Globals.PLUGIN_GUID, Globals.PLUGIN_NAME, Globals.PLUGIN_VERSION)]
    [BepInDependency("Terrain.MuckSettings")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;

        public Harmony harmony;

        private void Awake()
        {
            // Plugin startup logic
            Log = base.Logger;

            harmony = new Harmony(Globals.PLUGIN_NAME);

            harmony.PatchAll(typeof(SettingsPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched MuckSettings.Settings.Gameplay()");
            Log.LogInfo("Patched MuckSettings.Settings.Controls()");
            Log.LogInfo("Patched MuckSettings.Settings.Graphics()");
            Log.LogInfo("Patched MuckSettings.Settings.Video()");
            Log.LogInfo("Patched MuckSettings.Settings.Audio()");

            harmony.PatchAll(typeof(PagePatch.Page_Constructor));
            Log.LogInfo("Patched MuckSettings.Settings.Page()");

            harmony.PatchAll(typeof(SettingsMenuPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched MuckSettings.SettingsMenu.Prefix()");

            harmony.PatchAll(typeof(OriginalSettingsPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched Settings.Start()");

            harmony.PatchAll(typeof(ControlSettingsPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched MuckSettings.ControlSettings.Awake()");

            harmony.PatchAll(typeof(UpdateKeyListenerPatch.UpdateKeyListener_Prefix));
            Log.LogInfo("Patched MuckSettings.UpdateKeyListener.Prefix()");
        }
    }
}
