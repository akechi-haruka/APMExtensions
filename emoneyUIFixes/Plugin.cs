using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Apm.Emoney.Ui;
using Apm.Emoney.Ui.GamePad;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Emoney.SharedMemory;
using HarmonyLib;
using Haruka.Arcade.Apm.BananaphoneLib;
using Haruka.Arcade.Apm.EMUICF.External;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Color = Haruka.Arcade.SEGA835Lib.Misc.Color;
using Object = UnityEngine.Object;
using SceneManager = Apm.Emoney.Ui.SceneManager;

namespace Haruka.Arcade.Apm.EMUICF {
    [BepInPlugin("eu.haruka.apm.exmoneyui", "EMoneyUIExtended", "1.3")]
    [BepInProcess("emoneyUI")]
    [BepInDependency("eu.haruka.apm.headphone.emui", BepInDependency.DependencyFlags.SoftDependency)]
    [UsedImplicitly]
    public class Plugin : BaseUnityPlugin {
        // Scene for the custom menu
        public const SceneManager.State MOD_MENU_STATE = (SceneManager.State)5;

        public static ConfigEntry<bool> ConfigDebugMaxSize;
        public static ConfigEntry<bool> ConfigSpeakerAdjustmentEnabled;
        public static ConfigEntry<VolumeType> ConfigHeadphoneVolumeType;
        public static ConfigEntry<VolumeType> ConfigSpeakerVolumeType;
        public static ConfigEntry<bool> ConfigAllowExit;
        public static ConfigEntry<bool> ConfigEnableLedControl;
        public static ConfigEntry<bool> ConfigDisableTimeout;
        public static ConfigEntry<string> ConfigExDataPath;
        public static ConfigEntry<KeyboardShortcut> ConfigAppexReload;

        public static ManualLogSource Log;

        internal static AppExConfig AppExConfig;
        internal static AppExConfig.GuideInfo GuideData;
        internal static GeneralSetting ApmGeneralSetting;
        internal static SceneManager SceneManager;
        internal static Plugin Self;
        internal static LedManager LedManager;
        internal static float SavedSpeakerVolume = 50F;
        internal static float SavedHeadphoneVolume = 50F;

        [UsedImplicitly]
        public void Awake() {
            Log = Logger;
            Self = this;

            ConfigDebugMaxSize = Config.Bind("Debug", "Increase Size", false, new ConfigDescription("Sets window to 1000x1000 (for debugging)", null, new ConfigurationManagerAttributes() {
                IsAdvanced = true
            }));

            ConfigExDataPath = Config.Bind("Paths", "ExData Path", "W:\\AppEx", new ConfigDescription("Path to AppEx files.", null, new ConfigurationManagerAttributes() {
                IsAdvanced = true
            }));
            ConfigAppexReload = Config.Bind("Debug", "Reload AppEx", new KeyboardShortcut(KeyCode.F11), new ConfigDescription("This key reloads appex.json, useful for working on game guides.", null, new ConfigurationManagerAttributes() {
                IsAdvanced = true
            }));
            ConfigDisableTimeout = Config.Bind("Debug", "Disable Menu Timeout", false, new ConfigDescription("Disable the 30 second menu auto-close timeout.", null, new ConfigurationManagerAttributes() {
                IsAdvanced = true
            }));

            ConfigSpeakerAdjustmentEnabled = Config.Bind("General", "Enable Speaker Settings", true, "Allows players to change volume of the primary speakers");
            ConfigAllowExit = Config.Bind("General", "Enable Game Exit", true, "Allows players to exit game. Can be overridden from AppEx. See readme for more information.");

            ConfigSpeakerVolumeType = Config.Bind("General", "Speaker Level", VolumeType.Front, new ConfigDescription("The level (volume slider) for speaker output."));
            ConfigHeadphoneVolumeType = Config.Bind("General", "Headphone Level", VolumeType.Rear, new ConfigDescription("The level (volume slider) for headphone output."));

            ConfigEnableLedControl = Config.Bind("General", "Enable LED control", true, new ConfigDescription("Allows AppEx and players to control cabinet LEDs."));

            if (Headbanana.GetVersion() == Headbanana.EXPECTED_VERSION) {
                Headbanana.SetLogCallback(s => Log.LogInfo("BananaphoneLib: " + s));
                Headbanana.Initialize(ConfigSpeakerVolumeType.Value, ConfigHeadphoneVolumeType.Value);
            } else {
                Logger.LogError("Headbanana version invalid, expected " + Headbanana.EXPECTED_VERSION + ", got " + Headbanana.GetVersion());
            }

            Harmony.CreateAndPatchAll(typeof(Patches), "eu.haruka.gmg.apm.fixes.emoneyui.main");

            ReloadAppEx();
            ReloadGeneralSettings();

            if (ApmGeneralSetting.ledSetting?.portNumber > 0 && ConfigEnableLedControl.Value) {
                try {
                    LedManager = new LedManager(Log, ApmGeneralSetting.ledSetting.portNumber);
                    LedManager.Connect();
                    LedManager.Set(Color.FromArgb(AppExConfig.led.r, AppExConfig.led.g, AppExConfig.led.b));
                } catch (Exception ex) {
                    Log.LogError("Error setting up LED board: " + ex);
                }
            }

            Log.LogInfo("Loaded");
        }

        [UsedImplicitly]
        public void OnApplicationQuit() {
            LedManager?.Disconnect();
        }

        public static bool AllowLedControl {
            get { return ApmGeneralSetting.ledSetting?.portNumber > 0 && ConfigEnableLedControl.Value && !AppExConfig.led.block_user_change && (LedManager?.Connected ?? false); }
        }

        private void ReloadGeneralSettings() {
            string path = Path.Combine("Apmv3System_Data", "GeneralSetting.json");
            Log.LogInfo("Checking GeneralSetting at " + path + "...");
            if (File.Exists(path)) {
                try {
                    ApmGeneralSetting = JsonConvert.DeserializeObject<GeneralSetting>(File.ReadAllText(path));
                } catch (Exception ex) {
                    Log.LogError("Failed to read GeneralSetting: " + ex);
                }
            }

            Log.LogInfo("..." + (ApmGeneralSetting.ledSetting?.portNumber > 0 ? "success" : "failed"));
        }

        private void ReloadAppEx() {
            string path = Path.Combine(ConfigExDataPath.Value, "config.json");
            Log.LogInfo("Checking AppEx at " + path + "...");
            if (File.Exists(path)) {
                try {
                    AppExConfig = JsonConvert.DeserializeObject<AppExConfig>(File.ReadAllText(path));
                    Log.LogInfo("AppEx version: " + AppExConfig.version);
                } catch (Exception ex) {
                    Log.LogError("Failed to read AppEx: " + ex);
                }
            }

            Log.LogInfo("..." + (AppExConfig.version > 0 ? "success" : "failed"));

            path = Path.Combine(ConfigExDataPath.Value, "guide.json");
            Log.LogInfo("Checking Guide Data at " + path + "...");
            if (File.Exists(path)) {
                try {
                    GuideData = JsonConvert.DeserializeObject<AppExConfig.GuideInfo>(File.ReadAllText(path));
                } catch (Exception ex) {
                    Log.LogError("Failed to read Guide Data: " + ex);
                }
            }

            Log.LogInfo("..." + (GuideData.width > 0 ? "success" : "failed"));
        }

        [UsedImplicitly]
        public void Update() {
            if (ConfigAppexReload.Value.IsDown()) {
                ReloadAppEx();
                Log.LogMessage("AppEx reloaded");
            }
        }

        // not static because StartCoroutine requires some MonoBehaviour
        public void ChangePosAbsolute(RectTransform component, RectTransform from, Rect goal, float deltaTime) {
            Rect rect = new Rect(from.position.x, from.position.y, from.sizeDelta.x, from.sizeDelta.y);
            Log.LogDebug("ChangePosAbsolute: " + rect);
            Log.LogDebug("ChangePosAbsolute: " + goal);
            StartCoroutine(MoveAbsolute(component, rect, goal, deltaTime * 1.1F));
        }

        private IEnumerator MoveAbsolute(RectTransform target, Rect start, Rect goal, float deltaTime) {
            float startTime = Time.time;
            float count;
            do {
                count = deltaTime != 0f ? (Time.time - startTime) * 1f / deltaTime : 1f;
                float num = Mathf.Lerp(start.x, goal.x, count);
                float num2 = Mathf.Lerp(start.y, goal.y, count);
                float num3 = Mathf.Lerp(start.width, goal.width, count);
                float num4 = Mathf.Lerp(start.height, goal.height, count);
                target.transform.position = new Vector2(num, num2);
                target.sizeDelta = new Vector2(num3, num4);
                yield return null;
            } while (count <= 1f);
        }

        public static List<int> GetAudioChannels(string setting) {
            List<int> ret = new List<int>();
            foreach (string s in setting.Split(',')) {
                if (!String.IsNullOrWhiteSpace(s) && Int32.TryParse(s, out int ch)) {
                    ret.Add(ch);
                }
            }

            return ret;
        }

        public static void ExitGame(int _) {
            if (AppExConfig.exit.kill) {
                Log.LogInfo("Exiting via Process.Kill");
                string[] proclist = AppExConfig.exit.kill_process_name_list ?? Array.Empty<string>();
                foreach (string proc in proclist) {
                    foreach (Process p in Process.GetProcessesByName(proc)) {
                        Log.LogInfo("Killing: " + p.ProcessName);
                        p.Kill();
                    }
                }
            } else {
                Log.LogError("ExitGame failed: no method available");
            }
        }

        public static void OnModSettingsButton() {
            Log.LogDebug("Mod button clicked");
            SceneManager.MenuState = MOD_MENU_STATE;
        }

        public static void SaveCurrentAudioVolume() {
            float vol = Headbanana.GetSpeakerVolume();
            if (vol > 0) {
                SavedSpeakerVolume = vol;
            }

            vol = Headbanana.GetHeadphoneVolumeForDefault();
            if (vol > 0) {
                SavedHeadphoneVolume = vol;
            }
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Patches {
        private static GameObject modMenu;
        private static GameObject modIcon;
        private static GameObject modContents;
        private static GameObject modFrame;

        // crashfix
        [HarmonyPrefix, HarmonyPatch(typeof(ApmInputApi), "IsEqual")]
        static bool IsEqual(ref ApmInputApi.ApmGamepadConfig config1, ref ApmInputApi.ApmGamepadConfig config2, ref bool __result) {
            if (config1.Sw == null) {
                __result = false;
                return false;
            }

            return true;
        }

        #region Extra button and repositioning

        [HarmonyPrefix, HarmonyPatch(typeof(SceneManager), "Start")]
        static bool Start(SceneManager __instance) {
            UiSharedData sharedMemory = __instance.emoneyController.GetComponent<EmoneyController>().Data;

            // set this early for our purposes of shrinking/extending the window
            __instance.iconAxis = sharedMemory.Resource.EntryDirection == 0 ? GridLayoutGroup.Axis.Horizontal : GridLayoutGroup.Axis.Vertical;

            Plugin.Log.LogDebug("Menu size: X=" + __instance.entryMenuSize.x + ",Y=" + __instance.entryMenuSize.y);
            Plugin.Log.LogDebug("Icon alignment: " + __instance.iconAxis);

            // Add custom button and logic
            // why is this such a mess in Unity
            // I hate gamedev
            modMenu = Object.Instantiate(__instance.emoneyMenu.gameObject, __instance.mainCanvas.GetComponent<GridLayoutGroup>().transform);
            modMenu.transform.SetAsLastSibling();
            modMenu.name = "ModMenu";

            // Reduce hitbox of UI in minimized state (and account for new button)
            if (__instance.iconAxis == GridLayoutGroup.Axis.Vertical) {
                __instance.entryMenuSize.y += 60;
                __instance.entryMenuSize.x -= 160;
            } else if (__instance.iconAxis == GridLayoutGroup.Axis.Horizontal) {
                __instance.entryMenuSize.x += 100;
                __instance.entryMenuSize.y -= 100;
            }

            EmoneyMenu emenu = modMenu.GetComponent<EmoneyMenu>();
            MoreMenu menu = modMenu.AddComponent<MoreMenu>();
            menu.InitializeModdedObjectsFromCopy(emenu);
            Object.DestroyImmediate(emenu);

            modIcon = modMenu.transform.Find("Icon").gameObject;

            Button modButton = modIcon.transform.Find("Button").GetComponent<Button>();
            ModdingUtil.ChangeButton(modButton, Plugin.OnModSettingsButton);

            Image modImage = modButton.transform.Find("Image/frame").GetComponent<Image>();
            ModdingUtil.ChangeImage(modImage, Convert.FromBase64String(Images.MORE_BUTTON_TEXTURE), 74, 48);

            modFrame = modMenu.transform.Find("Frame").gameObject;
            modFrame = modMenu.transform.Find("Frame").gameObject;
            modContents = modFrame.transform.Find("Contents").gameObject;


            Plugin.SceneManager = __instance;

            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(WindowManager), "MoveWindow")]
        static void MoveWindow(ref Rect goal, float deltaTime, WindowManager.OnEnd onEnd = null) {
            Plugin.Log.LogDebug("MoveWindow: " + goal);
            if (Plugin.ConfigDebugMaxSize.Value) {
                goal.width = 1000;
                goal.height = 1000;
                goal.x = 0;
                goal.y = 0;
                Plugin.Log.LogInfo("Setting window to maximum size");
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(FrameManager), "ChangePos", typeof(Rect), typeof(float))]
        static void ChangePos(ref Rect goal, float deltaTime) {
            Plugin.Log.LogDebug("ChangePos: " + goal);
        }

        #endregion

        #region Mod button logic

        [HarmonyPostfix, HarmonyPatch(typeof(SceneManager), "OnEntryOpen")]
        static void OnEntryOpen() {
            modIcon.SetActive(true);
            modFrame.SetActive(true);
        }

        // reimplement because SEGA WTF IS SceneManager.GetFrameOffsetX/Y??
        // WHY ARE YOU CALCULATING THE OPEN WINDOW FRAME ***RELATIVE*** TO THE BUTTON POSITIONS!?
        [HarmonyPrefix, HarmonyPatch(typeof(SceneManager), "ChangeState")]
        static bool ChangeState(SceneManager.State pre, SceneManager.State next, SceneManager __instance) {
            GameObject currentFrame = null;
            switch (pre) {
                case SceneManager.State.HeadphoneMenu:
                    currentFrame = __instance.headphoneFrame;
                    break;
                case SceneManager.State.EmoneyMenu:
                    currentFrame = __instance.emoneyFrame;
                    __instance.emoneyController.GetComponent<EmoneyController>().ShowMainWindow = false;
                    break;
                case SceneManager.State.GamePadPreviewMenu:
                    currentFrame = __instance.gamePadFrame;
                    __instance.gamePadController.GetComponent<GamePadController>().ShowMainWindow = false;
                    break;
                case Plugin.MOD_MENU_STATE:
                    currentFrame = modFrame;
                    break;
            }

            switch (next) {
                case SceneManager.State.EntryMenu:
                    __instance.emoneyContents.SetActive(false);
                    __instance.headphoneContents.SetActive(false);
                    __instance.gamePadContents.SetActive(false);
                    modContents.SetActive(false);
                    __instance.EnableEmoneyIcon = __instance.availableEmoney;
                    __instance.EnableGamePadIcon = __instance.availableGamePad;
                    __instance.mainCanvas.GetComponent<GridLayoutGroup>().childAlignment = __instance.iconAlignment;
                    __instance.mainCanvas.GetComponent<GridLayoutGroup>().startAxis = __instance.iconAxis;
                    if (pre == SceneManager.State.Initial) {
                        __instance.windowManager.GetComponent<WindowManager>().ShowWindow(__instance.entryMenuRect, __instance.initialDelta, __instance.OnEntryOpen);
                        __instance.headphoneFrame.GetComponent<FrameManager>().Show(__instance.initialDelta);
                        __instance.emoneyFrame.GetComponent<FrameManager>().Show(__instance.initialDelta);
                        __instance.gamePadFrame.GetComponent<FrameManager>().Show(__instance.initialDelta);
                    } else {
                        __instance.windowManager.GetComponent<WindowManager>().EnableTransparency = true;
                        __instance.windowManager.GetComponent<WindowManager>().MoveWindow(__instance.entryMenuRect, __instance.delta, __instance.OnEntryOpen);
                        currentFrame?.GetComponent<FrameManager>().ChangePos(new Rect(0f, 0f, __instance.iconSize.x, __instance.iconSize.y), __instance.delta);
                    }

                    break;
                case SceneManager.State.HeadphoneMenu: {
                    __instance.emoneyFrame.SetActive(false);
                    __instance.emoneyIcon.SetActive(false);
                    __instance.gamePadFrame.SetActive(false);
                    __instance.gamePadIcon.SetActive(false);
                    modFrame.SetActive(false);
                    modIcon.SetActive(false);
                    __instance.headphoneIcon.SetActive(false);
                    __instance.headphoneFrame.SetActive(true);
                    __instance.headphoneContents.SetActive(true);

                    __instance.windowManager.GetComponent<WindowManager>().MoveWindow(__instance.mainMenuRect, __instance.delta, __instance.OnHeadphoneMenuOpen);

                    RectTransform target = __instance.headphoneFrame.transform.GetComponent<RectTransform>();
                    RectTransform from = __instance.headphoneIcon.transform.GetComponent<RectTransform>();
                    Plugin.Self.ChangePosAbsolute(target, from, new Rect(0, __instance.mainMenuRect.height, __instance.mainMenuRect.width, __instance.mainMenuRect.height), __instance.delta);


                    break;
                }
                case SceneManager.State.EmoneyMenu: {
                    __instance.headphoneFrame.SetActive(false);
                    __instance.headphoneIcon.SetActive(false);
                    __instance.gamePadFrame.SetActive(false);
                    __instance.gamePadIcon.SetActive(false);
                    modFrame.SetActive(false);
                    modIcon.SetActive(false);
                    __instance.emoneyIcon.SetActive(false);
                    __instance.emoneyFrame.SetActive(true);
                    __instance.emoneyContents.SetActive(true);

                    __instance.emoneyController.GetComponent<EmoneyController>().ShowMainWindow = true;
                    __instance.windowManager.GetComponent<WindowManager>().MoveWindow(__instance.mainMenuRect, __instance.delta, __instance.OnEmoneyMenuOpen);

                    RectTransform target = __instance.emoneyFrame.transform.GetComponent<RectTransform>();
                    RectTransform from = __instance.emoneyIcon.transform.GetComponent<RectTransform>();
                    Plugin.Self.ChangePosAbsolute(target, from, new Rect(0, __instance.mainMenuRect.height, __instance.mainMenuRect.width, __instance.mainMenuRect.height), __instance.delta);
                    break;
                }
                case SceneManager.State.GamePadPreviewMenu: {
                    __instance.headphoneFrame.SetActive(false);
                    __instance.headphoneIcon.SetActive(false);
                    __instance.emoneyFrame.SetActive(false);
                    __instance.emoneyIcon.SetActive(false);
                    modFrame.SetActive(false);
                    modIcon.SetActive(false);
                    __instance.gamePadIcon.SetActive(false);
                    __instance.gamePadFrame.SetActive(true);
                    __instance.gamePadContents.SetActive(true);

                    __instance.gamePadController.GetComponent<GamePadController>().ShowMainWindow = true;
                    __instance.windowManager.GetComponent<WindowManager>().MoveWindow(__instance.gamePadMenuRect, __instance.delta, __instance.OnGamePadMenuOpen);

                    RectTransform target = __instance.gamePadFrame.transform.GetComponent<RectTransform>();
                    RectTransform from = __instance.gamePadIcon.transform.GetComponent<RectTransform>();
                    Plugin.Self.ChangePosAbsolute(target, from, new Rect(0, __instance.gamePadMenuRect.height, __instance.gamePadMenuRect.width, __instance.gamePadMenuRect.height), __instance.delta);
                    break;
                }
                case Plugin.MOD_MENU_STATE: {
                    __instance.headphoneFrame.SetActive(false);
                    __instance.headphoneIcon.SetActive(false);
                    __instance.emoneyFrame.SetActive(false);
                    __instance.emoneyIcon.SetActive(false);
                    __instance.gamePadFrame.SetActive(false);
                    __instance.gamePadIcon.SetActive(false);
                    modIcon.SetActive(false);
                    modFrame.SetActive(true);
                    modContents.SetActive(true);

                    __instance.windowManager.GetComponent<WindowManager>().MoveWindow(__instance.mainMenuRect, __instance.delta, OnModMenuOpen);

                    RectTransform target = modFrame.transform.GetComponent<RectTransform>();
                    RectTransform from = modIcon.transform.GetComponent<RectTransform>();
                    Plugin.Self.ChangePosAbsolute(target, from, new Rect(0, __instance.mainMenuRect.height, __instance.mainMenuRect.width, __instance.mainMenuRect.height), __instance.delta);
                    break;
                }
            }

            __instance.menuState = next;
            return false;
        }

        private static void OnModMenuOpen() {
            modMenu.GetComponent<MoreMenu>().StartMenu();
            Plugin.SceneManager.windowManager.GetComponent<WindowManager>().EnableTransparency = false;
        }

        #endregion

        // don't update interactable flag in mod menu since these aren't linked to credits
        [HarmonyPrefix, HarmonyPatch(typeof(ItemButton), "Update")]
        static bool Update() {
            return Plugin.SceneManager.menuState != Plugin.MOD_MENU_STATE;
        }
    }
}