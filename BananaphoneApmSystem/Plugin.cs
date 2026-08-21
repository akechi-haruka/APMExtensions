using System.Diagnostics.CodeAnalysis;
using Apm.System.GameIconList;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Haruka.Arcade.Apm.BananaphoneLib;
using JetBrains.Annotations;

namespace Haruka.Arcade.Apm.Bananaphone.Apm;

[BepInPlugin("eu.haruka.apm.headphone.apm", "BananaphoneAPMSystem", "1.0")]
[BepInProcess("Apmv3System")]
public class Plugin : BaseUnityPlugin {
    internal new static ManualLogSource Logger;

    private static ConfigEntry<VolumeType> configHeadphoneLevel;

    [UsedImplicitly]
    private void Awake() {
        Logger = base.Logger;

        if (Headbanana.GetVersion() != Headbanana.EXPECTED_VERSION) {
            Logger.LogError("Headbanana version invalid, expected " + Headbanana.EXPECTED_VERSION + ", got " + Headbanana.GetVersion());
            return;
        }

        configHeadphoneLevel = Config.Bind("General", "Headphone Level", VolumeType.Rear, new ConfigDescription("The level (volume slider) for headphone output."));

        Headbanana.SetLogCallback(s => Logger.LogInfo("BananaphoneLib: " + s));
        Headbanana.Initialize(null, configHeadphoneLevel.Value);

        Harmony.CreateAndPatchAll(typeof(Patches), "eu.haruka.apm.headphone");

        Logger.LogInfo("Plugin is loaded!");
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Patches {
        [HarmonyPrefix, HarmonyPatch(typeof(HeadphoneMenu), "ApmHeadphoneVolumeGet")]
        static bool ApmHeadphoneVolumeGet(ref float __result) {
            __result = Headbanana.GetHeadphoneVolumeForDefault() / 2F;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(HeadphoneMenu), "ApmHeadphoneVolumeSet")]
        static bool ApmHeadphoneVolumeSet(float volume) {
            Headbanana.SetHeadphoneVolumeForDefault(volume * 2F);
            return false;
        }
    }
}