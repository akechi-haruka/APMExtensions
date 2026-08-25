using System.Collections.Generic;
using BepInEx.Logging;
using Haruka.Arcade.SEGA835Lib.Debugging;
using Haruka.Arcade.SEGA835Lib.Devices;
using Haruka.Arcade.SEGA835Lib.Devices.LED._837_15093;
using Haruka.Arcade.SEGA835Lib.Misc;
using Haruka.Logging.BepInEx;
using Microsoft.Extensions.Logging;

namespace Haruka.Arcade.Apm.EMUICF.External {
    public class LedManager {
        private const int LED_COUNT = 66;

        private static bool initialized;

        public bool Connected { get; private set; }

        private readonly Led15093 board;
        private readonly ManualLogSource log;
        private Color current;

        public LedManager(ManualLogSource log, int ledSettingPortNumber) {
            log.LogDebug("Creating LedManager with port " + ledSettingPortNumber);
            if (!initialized) {
                LogManager.Initialize(LoggerFactory.Create(builder => { builder.AddBepInEx(log); }));
                initialized = true;
            }

            this.log = log;
            board = new Led15093(ledSettingPortNumber);
        }

        public void Connect() {
            if (board.Connect() != DeviceStatus.Ok) {
                log.LogError("Failed to connect to LED board");
                return;
            }

            if (board.SetResponseDisabled(true) != DeviceStatus.Ok) {
                log.LogError("Failed to update response setting");
                return;
            }

            Connected = true;
            log.LogInfo("LED board connected");
        }

        public void Disconnect() {
            board?.Disconnect();
        }

        public void Set(UnityEngine.Color c) {
            Set(Color.FromArgb((byte)(c.r * 255), (byte)(c.g * 255), (byte)(c.b * 255)));
        }

        public void Set(Color c) {
            if (!Connected) {
                return;
            }

            if (c.R == current.R && c.G == current.G && c.B == current.B) {
                return;
            }

            current = c;

            List<Color> colors = new List<Color>();
            for (int i = 0; i < LED_COUNT; i++) {
                colors.Add(c);
            }

            if (board.SetLeds(colors) != DeviceStatus.Ok) {
                log.LogError("SetLeds error");
                Connected = false;
            }
        }
    }
}