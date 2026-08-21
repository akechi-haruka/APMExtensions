using System;
using System.Collections.Generic;
using Haruka.Arcade.Apm.BananaphoneLib.MMDevice;
using NAudio.CoreAudioApi;
using NAudio.Wasapi.CoreAudioApi;

namespace Haruka.Arcade.Apm.BananaphoneLib {
    public static class Headbanana {
        public const int EXPECTED_VERSION = 5;

        internal static Action<string> Log = _ => { };
        private static VolumeType? masterType;
        private static VolumeType? headphoneType;
        private static IBanana impl;
        private static readonly bool USE_COM = Type.GetType("Mono.Runtime") == null;

        public static int GetVersion() {
            return EXPECTED_VERSION;
        }

        public static void SetLogCallback(Action<string> callback) {
            Log = callback ?? throw new NullReferenceException("callback is null");
        }

        public static bool IsAvailable() {
            return impl != null;
        }

        public static void Initialize(VolumeType? masterLevel, VolumeType? headphoneLevel) {
            impl?.Dispose();
            if (USE_COM) {
                impl = new BananaCom();
            } else {
                impl = new BananaNAudio();
            }

            Log("Initializing: " + impl);

            try {
                impl.Initialize();
            } catch (Exception ex) {
                Log("Failed to initialize " + impl + ": " + ex);
                impl = null;
            }

            masterType = masterLevel;
            headphoneType = headphoneLevel;
        }

        public static SoundVolume.Channel ConvertTypeToChannel(VolumeType volumeType, bool left = true) {
            switch (volumeType) {
                case VolumeType.Front:
                    return left ? SoundVolume.Channel.FrontLeft : SoundVolume.Channel.FrontRight;
                case VolumeType.Rear:
                    return left ? SoundVolume.Channel.RearLeft : SoundVolume.Channel.RearRight;
                case VolumeType.Center:
                    return SoundVolume.Channel.Center;
                case VolumeType.Subwoofer:
                    return SoundVolume.Channel.Woofer;
                default:
                    throw new ArgumentException("Unknown volumeType: " + volumeType);
            }
        }

        public static float GetHeadphoneVolumeForDefault() {
            if (IsAvailable() && headphoneType != null) {
                return impl.GetVolume(headphoneType.Value);
            }

            return 0;
        }

        public static void SetHeadphoneVolumeForDefault(float volume) {
            if (IsAvailable() && headphoneType != null) {
                impl.SetVolume(headphoneType.Value, volume);
            }
        }

        public static float GetSpeakerVolume() {
            if (IsAvailable() && masterType != null) {
                return impl.GetVolume(masterType.Value);
            }

            return 0;
        }

        public static void SetSpeakerVolume(float volume) {
            if (IsAvailable() && masterType != null) {
                impl.SetVolume(masterType.Value, volume);
            }
        }
    }

    public interface IBanana {
        void Initialize();

        void Dispose();

        float GetVolume(VolumeType volumeType);

        void SetVolume(VolumeType volumeType, float volume);
    }

    public class BananaCom : IBanana {
        private static SoundVolume sound;

        public void Initialize() {
            sound = new SoundVolume();
            if (!sound.Available) {
                throw new Exception("Initialization failed");
            }
        }

        public void Dispose() {
            sound.Dispose();
        }

        public float GetVolume(VolumeType volumeType) {
            if (sound.Available) {
                if (sound.GetVolume(Headbanana.ConvertTypeToChannel(volumeType), out uint vol)) {
                    return vol;
                }

                Headbanana.Log("Failed to retrieve sound volume for " + volumeType + ": error");
            } else {
                Headbanana.Log("Failed to retrieve sound volume for " + volumeType + ": uninitialized");
            }

            return 0F;
        }

        public void SetVolume(VolumeType volumeType, float volume) {
            if (sound.Available) {
                SoundVolume.Channel t1 = Headbanana.ConvertTypeToChannel(volumeType, false);
                SoundVolume.Channel t2 = Headbanana.ConvertTypeToChannel(volumeType);
                if (!sound.SetVolume(t1, (uint)volume)) {
                    Headbanana.Log("Failed to set sound volume for " + volumeType + "/right: error");
                }

                if (t1 != t2) {
                    if (!sound.SetVolume(t2, (uint)volume)) {
                        Headbanana.Log("Failed to set sound volume for " + volumeType + "/left: error");
                    }
                }
            } else {
                Headbanana.Log("Failed to set volume for " + volumeType + ": uninitialized");
            }
        }
    }

    public class BananaNAudio : IBanana {
        private NAudio.CoreAudioApi.MMDevice device;
        private readonly Dictionary<VolumeType, AudioVolumeLevel> channels = new Dictionary<VolumeType, AudioVolumeLevel>();
        private DeviceTopology top;

        public void Initialize() {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            top = device.DeviceTopology;
            Headbanana.Log("Connector Count: " + top.ConnectorCount);
            for (uint i = 0; i < top.ConnectorCount; i++) {
                Connector conn = top.GetConnector(i);
                Headbanana.Log("Connector " + i + ": " + conn.Part.Name);
                Part part = conn.ConnectedTo.Part;
                ProcessPart(part);
            }

            Headbanana.Log("Channel setup:");
            foreach (VolumeType t in Enum.GetValues(typeof(VolumeType))) {
                Headbanana.Log(t + ": " + (channels[t]?.ToString() ?? "NOT AVAILABLE"));
            }

            enumerator.Dispose();
        }

        private void ProcessPart(Part part) {
            AudioVolumeLevel channel = part.AudioVolumeLevel;
            if (channel != null) {
                switch (part.Name) {
                    case nameof(SoundVolume.VolumeControlName.Front):
                        Headbanana.Log("Front channel found: " + part.GlobalId);
                        channels[VolumeType.Front] = channel;
                        break;
                    case nameof(SoundVolume.VolumeControlName.Rear):
                        Headbanana.Log("Rear channel found: " + part.GlobalId);
                        channels[VolumeType.Rear] = channel;
                        break;
                    case nameof(SoundVolume.VolumeControlName.Center):
                        Headbanana.Log("Center channel found: " + part.GlobalId);
                        channels[VolumeType.Center] = channel;
                        break;
                    case nameof(SoundVolume.VolumeControlName.Subwoofer):
                        Headbanana.Log("Subwoofer channel found: " + part.GlobalId);
                        channels[VolumeType.Subwoofer] = channel;
                        break;
                    default:
                        Headbanana.Log("Unknown control, skipping: " + part.Name);
                        break;
                }
            }

            PartsList parts = part.PartsIncoming;
            uint count = parts.Count;
            for (uint i = 0; i < count; i++) {
                ProcessPart(parts[i]);
            }
        }

        public void Dispose() {
            device?.Dispose();
        }

        public float GetVolume(VolumeType volumeType) {
            if (channels.TryGetValue(volumeType, out AudioVolumeLevel level)) {
                float vol = level.GetLevel(0);
                Headbanana.Log("Channel Count: " + level.ChannelCount);

                float targetVolume = (float)(Math.Pow(10.0, vol / 20.0) * 100.0);

                Headbanana.Log(volumeType + " <- " + vol + " (" + targetVolume + ")");
                return targetVolume;
            }

            Headbanana.Log("Channel not set (get): " + volumeType);
            return 0F;
        }

        public void SetVolume(VolumeType volumeType, float volume) {
            if (channels.TryGetValue(volumeType, out AudioVolumeLevel level)) {
                level.GetLevelRange(0, out float min, out float max, out float step);
                float targetVolume;
                if (volume > 0U) {
                    targetVolume = (float)Math.Log10(volume / 100.0) * 20f;
                    targetVolume += step;
                } else {
                    targetVolume = min;
                }

                if (targetVolume < min) {
                    targetVolume = min;
                } else if (targetVolume > max) {
                    targetVolume = max;
                }

                level.SetLevelUniform(targetVolume);
                Headbanana.Log(volumeType + " -> " + volume + " (" + targetVolume + ")");
            } else {
                Headbanana.Log("Channel not set (set): " + volumeType);
            }
        }
    }
}