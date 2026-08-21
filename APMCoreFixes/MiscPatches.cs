using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using AMDaemon;
using Apm.System.AbaasGs;
using Apm.System.GameIconList;
using Apm.System.Setting.Volatile;
using Apm.System.UnityUtil;
using Apm.System.Util.Log;
using Apm.System.Warning;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using static Apm.System.Daemon.Input;
using static Apm.System.Error.ErrorResource;
using Object = UnityEngine.Object;
using SceneManager = Apm.System.GameIconList.SceneManager;

namespace APMCoreFixes {
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    class MiscPatches {
        // Skip warning screen
        [HarmonyPrefix, HarmonyPatch(typeof(Warning), "StartAnimation")]
        static bool StartAnimation(AnimationController.AnimationEnd onEnd) {
            if (ApmCoreFixes.ConfigSkipWarning.Value) {
                onEnd();
                return false;
            }

            return true;
        }

        private static void DeleteVirtualDrive(string letter) {
            ApmCoreFixes.Log.LogDebug("Deleting virtual drive (if any)");
            try {
                Process p = Process.Start(new ProcessStartInfo("subst.exe", letter + ": /D") {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });
                p.ErrorDataReceived += P_ErrorDataReceived;
                p.OutputDataReceived += P_OutputDataReceived;
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();
            } catch (Exception ex) {
                ApmCoreFixes.Log.LogError("Failed to set virtual drive: " + ex);
                Error.Set((int)ErrorNumber.CommonUnexpectedGameProgramFailure);
            }
        }

        private static bool SetVirtualDrive(string letter, string gamePath) {
            ApmCoreFixes.Log.LogDebug("Setting virtual drive");
            try {
                Process p = Process.Start(new ProcessStartInfo("subst.exe", letter + ": " + gamePath) {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });
                p.ErrorDataReceived += P_ErrorDataReceived;
                p.OutputDataReceived += P_OutputDataReceived;
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();
                if (p.ExitCode != 0) {
                    throw new Exception("Return code of subst is " + p.ExitCode);
                }

                return true;
            } catch (Exception ex) {
                ApmCoreFixes.Log.LogError("Failed to set virtual drive: " + ex);
                Error.Set((int)ErrorNumber.CommonUnexpectedGameProgramFailure);
                return false;
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Apm.System.Setup.SceneManager), "Update")]
        public static bool Update(Apm.System.Setup.SceneManager __instance) {
            if (!ApmCoreFixes.ConfigUseBatchLaunchSystem.Value) {
                return true;
            }

            if (__instance.state != Apm.System.Setup.SceneManager.State.StartGame) {
                return true;
            }

            ApmCoreFixes.Log.LogInfo("Launching " + __instance.subGameId + "...");
            AppInfo game = AppListManager.GetInstance().Info.List.Find(p => p.subGameId == __instance.subGameId);
            if (game == null) {
                ApmCoreFixes.Log.LogError("No such game entry: " + __instance.subGameId);
                Error.Set((int)ErrorNumber.ApmUnexpectedGameProgramFailure);
                return false;
            }

            string gamePath = Directory.GetParent(game.paths.images.Original).FullName;

            ApmCoreFixes.Log.LogInfo("Target Path: " + gamePath);

            if (!File.Exists(Path.Combine(gamePath, "game.bat"))) {
                ApmCoreFixes.Log.LogWarning("No game.bat in root directory found, falling back to actual start routine!");
                return true;
            }

            DeleteVirtualDrive("W");
            if (!SetVirtualDrive("W", gamePath)) {
                return false;
            }

            ApmCoreFixes.Log.LogDebug("Virtual drive set");

            __instance.state = Apm.System.Setup.SceneManager.State.WaitStartGame;
            ApmCoreFixes.Log.LogDebug("OnMountEnd");
            __instance.OnMountEnd(true);
            ApmCoreFixes.Log.LogDebug("OnStartGameEnd");
            __instance.OnStartGameEnd(true);
            ApmCoreFixes.Log.LogDebug("OK");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(SceneManager), "GameStart")]
        public static bool GameStart(string subGameId, string version, AppAdditionalInfo info, SceneManager __instance) {
            if (!ApmCoreFixes.ConfigUseBatchLaunchSystem.Value) {
                return true;
            }

            ApmCoreFixes.Log.LogInfo("Launching " + subGameId + "...");
            AppInfo game = AppListManager.GetInstance().Info.List.Find(p => p.subGameId == subGameId);
            if (game == null) {
                ApmCoreFixes.Log.LogError("No such game entry: " + subGameId);
                Error.Set((int)ErrorNumber.ApmUnexpectedGameProgramFailure);
                return false;
            }

            string gamePath = Directory.GetParent(game.paths.images.Original).FullName;

            ApmCoreFixes.Log.LogInfo("Target Path: " + gamePath);

            if (!File.Exists(Path.Combine(gamePath, "game.bat"))) {
                ApmCoreFixes.Log.LogWarning("No game.bat in root directory found, falling back to actual start routine!");
                return true;
            }

            Thread.Sleep(1000); // let sound effect finish

            DeleteVirtualDrive("W");
            if (!SetVirtualDrive("W", gamePath)) {
                return false;
            }

            ApmCoreFixes.Log.LogDebug("Virtual drive set");

            __instance.isStartingGame = true;
            __instance.launchSubGameId = subGameId;
            __instance.launchVersion = version;
            if (!__instance.bootApplication) {
                __instance.isMountEnd = true;
                __instance.isStartGameEnd = true;
                return false;
            }

            SystemConfigManager.GetInstance().Info.EMoney = info.EMoney;
            SystemConfigManager.GetInstance().Info.Ui = info.Ui;
            SystemConfigManager.GetInstance().Info.GamePad = info.GamePad;
            PlayLogSender.Save("Launch " + subGameId + " Ver." + version);
            ApmCoreFixes.Log.LogDebug("Cancel Network");
            __instance.abaasGsController.GetComponent<Main>().Cancel();
            ApmCoreFixes.Log.LogDebug("OnMountEnd");
            __instance.OnMountEnd(true);
            ApmCoreFixes.Log.LogDebug("OnStartGameEnd");
            __instance.OnStartGameEnd(true);
            ApmCoreFixes.Log.LogDebug("OK");
            return false;
        }

        private static void P_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            ApmCoreFixes.Log.LogInfo("External: " + e.Data);
        }

        private static void P_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            ApmCoreFixes.Log.LogError("External: " + e.Data);
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Apm.System.Warning.SceneManager), "OnStartGameEnd")]
        static bool OnStartGameEnd(Apm.System.Warning.SceneManager __instance, bool isSucceeded) {
            if (isSucceeded) {
                __instance.isStartGameEnd = true;
            } else {
                ApmCoreFixes.Log.LogError("Game start not successful (Warning)");
            }

            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Apm.System.Setup.SceneManager), "OnStartGameEnd")]
        static bool OnStartGameEnd(Apm.System.Setup.SceneManager __instance, bool isSucceeded) {
            if (isSucceeded) {
                __instance.isStartGameEnd = true;
            } else {
                ApmCoreFixes.Log.LogError("Game start not successful (Setup)");
            }

            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(SceneManager), "OnStartGameEnd")]
        static bool OnStartGameEnd(SceneManager __instance, bool isSucceeded) {
            if (isSucceeded) {
                __instance.isStartGameEnd = true;
            } else {
                ApmCoreFixes.Log.LogError("Game start not successful (GameList)");
            }

            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Apm.System.Daemon.Main), "IsRebootNeeded", MethodType.Getter)]
        static bool IsRebootNeeded(ref bool __result) {
            if (ApmCoreFixes.ConfigIgnoreReboots.Value) {
                __result = false;
                return false;
            }

            return true;
        }

        // reimplement to allow more than 40 games
        [HarmonyPrefix, HarmonyPatch(typeof(IconListGridCanvas), "Start")]
        static bool Start(IconListGridCanvas __instance) {
            foreach (object obj in __instance.gameObject.transform) {
                Object.Destroy(((Transform)obj).gameObject);
            }

            List<AppInfo> list = (from info in AppListManager.GetInstance().Info.SelectableList
                where info.New
                orderby info.StartDate descending
                select info).ToList();
            list.AddRange((from info in AppListManager.GetInstance().Info.SelectableList
                where !info.New
                orderby info.StartDate descending
                select info).ToList());

            bool moddedPrefab = false;
            GridLayoutGroup component = __instance.GetComponent<GridLayoutGroup>();
            if (list.Count > 40) {
                __instance.iconPrefab = __instance.iconPrefab40;
                component.cellSize = new Vector2(115.5f, 115.5f);
                component.spacing = new Vector2(50f, 47.5f);
                component.constraintCount = 11;
                __instance.iconCountMaxHorizontal = 11;
                moddedPrefab = true;
            } else if (list.Count > 24 || __instance.grid40Test) {
                __instance.iconPrefab = __instance.iconPrefab40;
                component.cellSize = new Vector2(135f, 135f);
                component.spacing = new Vector2(50f, 65f);
                component.constraintCount = 10;
                __instance.iconCountMaxHorizontal = 10;
            } else {
                __instance.iconPrefab = __instance.iconPrefab24;
                component.cellSize = new Vector2(180f, 180f);
                component.spacing = new Vector2(50f, 90f);
                component.constraintCount = 8;
                __instance.iconCountMaxHorizontal = 8;
            }

            int count = 0;
            foreach (AppInfo appInfo in list) {
                IconGridCanvas component2 = Object.Instantiate(__instance.iconPrefab, __instance.transform).GetComponent<IconGridCanvas>();
                component2.name = appInfo.SubGameId;
                component2.Version = appInfo.Version;
                component2.EMoney = appInfo.EMoney;
                component2.Ui = appInfo.Ui;
                component2.GamePad = appInfo.GamePad;
                component2.VideoPanelAnim = __instance.videoPanel;
                component2.Sound = __instance.soundManager;
                component2.OnGameStart.AddListener(__instance.PlayButtonClick);
                component2.ResetAdvertizeElapsedTime = __instance.resetAdvertizeElapsedTime;
                component2.IsEnableNewIcon(appInfo.New);
                Texture2D texture2D = new DynamicPngTexture().ReadFileAsTexture(appInfo.Paths.Icon);
                component2.AppIcon.GetComponent<Image>().sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), Vector2.zero);
                Transform text = component2.AppIcon.transform.Find("TextCanvas/TextArea/TitleText");
                text.GetComponent<Text>().text = appInfo.TitleName;
                if (moddedPrefab) {
                    text.parent.transform.localPosition = new Vector3(-57.75F, -67.25F, 0);
                    text.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(97, 24);
                }

                component2.SetMediaFile(appInfo.SubGameId);
                __instance.iconsOrder.Add(component2);

                if (++count >= 55) {
                    break;
                }
            }

            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(InputSystem), "Update")]
        static bool Update(InputSystem __instance) {
            if (ApmCoreFixes.ConfigAmdAnalogInsteadOfButtons.Value) {
                if (__instance.sw == InputSwitch.up || __instance.sw == InputSwitch.right || __instance.sw == InputSwitch.down || __instance.sw == InputSwitch.left) {
                    UpdateAnalog(__instance);
                    return false;
                }
            }

            return true;
        }

        private static double Map(double x, double in_min, double in_max, double out_min, double out_max) {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        private static void UpdateAnalog(InputSystem input) {
            InputUnit unit = Input.Players[0];

            double deadzone = ApmCoreFixes.ConfigIo4StickDeadzone.Value / 100F;
            var ax = unit.GetAnalog(ApmCoreFixes.AnalogX).Value;
            var ay = unit.GetAnalog(ApmCoreFixes.AnalogY).Value;
            double x = Map(ax, 0, 1, -1, 1);
            double y = Map(ay, 0, 1, -1, 1);

            if (ApmCoreFixes.ConfigIo4AxisXInvert.Value) {
                x = -x;
            }

            if (ApmCoreFixes.ConfigIo4AxisYInvert.Value) {
                y = -y;
            }

            bool on = (
                (input.sw == InputSwitch.up && y > deadzone) ||
                (input.sw == InputSwitch.right && x > deadzone) ||
                (input.sw == InputSwitch.down && y < -deadzone) ||
                (input.sw == InputSwitch.left && x < -deadzone)
            );

            if (on) {
                IsOn isOn = input.events.IsOn;
                if (isOn != null) {
                    isOn(input.sw);
                }
            } else {
                IsOff isOff = input.events.IsOff;
                if (isOff != null) {
                    isOff(input.sw);
                }
            }
        }
    }
}