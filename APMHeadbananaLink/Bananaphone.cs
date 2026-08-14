using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using JetBrains.Annotations;

namespace Haruka.Arcade.APMHeadbanana {
    [BepInPlugin("eu.haruka.apm.bananaphone", "HeadbananaLink", "1.3")]
    public class Bananaphone : BaseUnityPlugin {
        public ConfigEntry<string> ConfigChannelList;
        public ConfigEntry<bool> ConfigFullRange;

        private static int version;

        public static bool IsWorking {
            get { return version > 0; }
        }

        [UsedImplicitly]
        public void Awake() {
            ConfigChannelList = Config.Bind("General", "Headphone Channels", "2,3", "A comma seperated list of channels that should be manipulated by APMHeadbanana");
            ConfigFullRange = Config.Bind("General", "Use Full Range", false, "By default, the headphone audio slider only goes up to 50% system volume, this will make it go up to 100%. Requires version 2.");
            ConfigChannelList.SettingChanged += ConfigChannelList_SettingChanged;
            ConfigFullRange.SettingChanged += ConfigChannelList_SettingChanged;

            try {
                version = Native.ApmHeadbananaVersionGet();
                Logger.LogInfo("BANANA: version " + version);
            } catch {
                Logger.LogError("NO BANANA.");
                return;
            }

            UpdateChannels();
        }

        private void ConfigChannelList_SettingChanged(object sender, EventArgs e) {
            UpdateChannels();
        }

        private void UpdateChannels() {
            List<int> channels = new List<int>();
            foreach (string s in ConfigChannelList.Value.Split(',')) {
                if (Int32.TryParse(s, out int channel)) {
                    channels.Add(channel);
                } else {
                    Logger.LogWarning("Could not parse channel: " + s);
                }
            }

            if (channels.Count > 0) {
                Native.ApmHeadphoneChannelsSet(channels.ToArray(), channels.Count);
                Logger.LogDebug("Channel list updated");
            } else {
                Logger.LogError("Channel list is empty");
            }

            if (version >= 2) {
                Native.ApmHeadphoneVolumeSetFullRange(ConfigFullRange.Value);
            }
        }
    }
}