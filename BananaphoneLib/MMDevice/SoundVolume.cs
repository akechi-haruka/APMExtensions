using System;
using System.Runtime.InteropServices;

namespace Haruka.Arcade.Apm.BananaphoneLib.MMDevice {
    public class SoundVolume : IDisposable {
        public SoundVolume() {
            iAudioEndpointVolumeGuid = typeof(IAudioEndpointVolume).GUID;
            iDeviceTopologyGuid = typeof(IDeviceTopology).GUID;
            iPartGuid = typeof(IPart).GUID;
            iAudioVolumeLevelGuid = typeof(IAudioVolumeLevel).GUID;
            ksNodeTypeVolume = new Guid("3A5ACC00-C557-11D0-8A2B-00A0C9255AC1");
            channels = new IAudioVolumeLevel[4];
            master = null;
            Available = Init();
        }

        public void Dispose() {
            foreach (IAudioVolumeLevel audioVolumeLevel in channels) {
                if (audioVolumeLevel != null) {
                    Marshal.ReleaseComObject(audioVolumeLevel);
                }
            }

            if (master != null) {
                Marshal.ReleaseComObject(master);
            }
        }

        public bool SetMasterVolume(uint level) {
            if (!Available) {
                return false;
            }

            master.GetChannelCount(out int num);
            uint num2 = 0U;
            while (num2 < (ulong)num) {
                master.SetChannelVolumeLevelScalar(num2, level / 100f, Guid.Empty);
                num2 += 1U;
            }

            return true;
        }

        public bool GetMasterVolume(out uint level) {
            level = 0U;
            if (!Available) {
                return false;
            }

            master.GetChannelCount(out int num);
            uint num2 = 0U;
            while (num2 < (ulong)num) {
                master.GetChannelVolumeLevelScalar(num2, out float num3);
                if ((uint)(num3 * 100f) > level) {
                    level = (uint)(num3 * 100f);
                }

                num2 += 1U;
            }

            return true;
        }

        public bool SetVolume(Channel ch, uint level) {
            if (!Available) {
                return false;
            }

            IAudioVolumeLevel audioVolumeLevel = null;
            switch (ch) {
                case Channel.FrontLeft:
                case Channel.FrontRight:
                    audioVolumeLevel = channels[0];
                    break;
                case Channel.RearLeft:
                case Channel.RearRight:
                    audioVolumeLevel = channels[1];
                    break;
                case Channel.Center:
                    audioVolumeLevel = channels[2];
                    break;
                case Channel.Woofer:
                    audioVolumeLevel = channels[3];
                    break;
            }

            if (audioVolumeLevel == null) {
                return false;
            }

            uint num = ch == Channel.FrontRight || ch == Channel.RearRight ? 1U : 0U;
            audioVolumeLevel.GetLevelRange(num, out float num2, out float num3, out float num4);
            float num5;
            if (level > 0U) {
                num5 = (float)Math.Log10(level / 100.0) * 20f;
                num5 += num4;
            } else {
                num5 = num2;
            }

            if (num5 < num2) {
                num5 = num2;
            } else if (num5 > num3) {
                num5 = num3;
            }

            audioVolumeLevel.SetLevel(num, num5, Guid.Empty);
            return true;
        }

        public bool GetVolume(Channel ch, out uint level) {
            level = 0U;
            if (!Available) {
                return false;
            }

            IAudioVolumeLevel audioVolumeLevel = null;
            switch (ch) {
                case Channel.FrontLeft:
                case Channel.FrontRight:
                    audioVolumeLevel = channels[0];
                    break;
                case Channel.RearLeft:
                case Channel.RearRight:
                    audioVolumeLevel = channels[1];
                    break;
                case Channel.Center:
                    audioVolumeLevel = channels[2];
                    break;
                case Channel.Woofer:
                    audioVolumeLevel = channels[3];
                    break;
            }

            if (audioVolumeLevel == null) {
                Headbanana.Log("Unknown channel passed");
                return false;
            }

            uint num = ch == Channel.FrontRight || ch == Channel.RearRight ? 1U : 0U;
            audioVolumeLevel.GetLevel(num, out float num2);
            level = (uint)(Math.Pow(10.0, num2 / 20.0) * 100.0);
            return true;
        }

        private bool Init() {
            Headbanana.Log("Initializing WASAPI");
            IMMDeviceEnumerator immdeviceEnumerator = MMDeviceEnumeratorFactory.CreateInstance();
            IMMDevice immdevice = null;
            IDeviceTopology deviceTopology = null;
            IConnector connector = null;
            IConnector connector2 = null;
            IPart part = null;
            try {
                Marshal.ThrowExceptionForHR(immdeviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.ERender, ERole.EMultimedia, out immdevice));
                Marshal.ThrowExceptionForHR(immdevice.Activate(ref iDeviceTopologyGuid, 1, IntPtr.Zero, out object obj));
                deviceTopology = obj as IDeviceTopology;
                Marshal.ThrowExceptionForHR(deviceTopology.GetConnector(0U, out connector));
                Marshal.ThrowExceptionForHR(connector.GetConnectedTo(out connector2));
                Marshal.QueryInterface(Marshal.GetIUnknownForObject(connector2), ref iPartGuid, out IntPtr zero);
                part = (IPart)Marshal.GetObjectForIUnknown(zero);
                Marshal.ThrowExceptionForHR(immdevice.Activate(ref iAudioEndpointVolumeGuid, 1, IntPtr.Zero, out obj));
                master = obj as IAudioEndpointVolume;
                Collect(ref part);
            } catch (Exception ex) {
                Headbanana.Log("Exception occurred: " + ex);
                return false;
            } finally {
                if (immdeviceEnumerator != null) {
                    Marshal.ReleaseComObject(immdeviceEnumerator);
                }

                if (immdevice != null) {
                    Marshal.ReleaseComObject(immdevice);
                }

                if (deviceTopology != null) {
                    Marshal.ReleaseComObject(deviceTopology);
                }

                if (connector != null) {
                    Marshal.ReleaseComObject(connector);
                }

                if (connector2 != null) {
                    Marshal.ReleaseComObject(connector2);
                }

                if (part != null) {
                    Marshal.ReleaseComObject(part);
                }
            }

            Headbanana.Log("Initialization succeeded");

            return true;
        }

        private void Collect(ref IPart iPart) {
            if (iPart == null) {
                return;
            }

            iPart.GetSubType(out Guid guid);
            if (guid == ksNodeTypeVolume) {
                iPart.GetName(out string text);
                iPart.Activate(CLSCTX.INPROC_SERVER, ref iAudioVolumeLevelGuid, out object obj);
                switch (text) {
                    case nameof(VolumeControlName.Front):
                        channels[0] = obj as IAudioVolumeLevel;
                        break;
                    case nameof(VolumeControlName.Rear):
                        channels[1] = obj as IAudioVolumeLevel;
                        break;
                    case nameof(VolumeControlName.Center):
                        channels[2] = obj as IAudioVolumeLevel;
                        break;
                    case nameof(VolumeControlName.Subwoofer):
                        channels[3] = obj as IAudioVolumeLevel;
                        break;
                }
            }

            IPartsList partsList;
            try {
                Marshal.ThrowExceptionForHR(iPart.EnumPartsIncoming(out partsList));
            } catch {
                return;
            }

            partsList.GetCount(out uint count);
            for (uint i = 0U; i < count; i += 1U) {
                IPart part = null;
                try {
                    Marshal.ThrowExceptionForHR(partsList.GetPart(i, out part));
                    if (part != null) {
                        Collect(ref part);
                        Marshal.ReleaseComObject(part);
                    }
                } catch (Exception ex) {
                    Headbanana.Log("Failed to collect part: " + part + ": " + ex);
                }
            }

            Marshal.ReleaseComObject(partsList);
        }

        private Guid iAudioEndpointVolumeGuid;

        private Guid iDeviceTopologyGuid;

        private Guid iPartGuid;

        private Guid iAudioVolumeLevelGuid;

        private readonly Guid ksNodeTypeVolume;

        private readonly IAudioVolumeLevel[] channels;

        private IAudioEndpointVolume master;

        public bool Available;

        public enum Channel : uint {
            FrontLeft,
            FrontRight,
            RearLeft,
            RearRight,
            Center,
            Woofer
        }

        public enum VolumeControlName : uint {
            Front,
            Rear,
            Center,
            Subwoofer
        }
    }
}