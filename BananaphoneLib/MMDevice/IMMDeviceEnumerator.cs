using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Haruka.Arcade.Apm.BananaphoneLib.MMDevice {
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    interface IMMDeviceEnumerator {
        int NotImpl1();

        [PreserveSig]
        int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppDevice);
    }

    static class MMDeviceEnumeratorFactory {
        private static readonly Guid MM_DEVICE_ENUMERATOR = new Guid("BCDE0395-E52F-467C-8E3D-C4579291692E");

        public static IMMDeviceEnumerator CreateInstance() {
            Type type = Type.GetTypeFromCLSID(MM_DEVICE_ENUMERATOR);
            return (IMMDeviceEnumerator)Activator.CreateInstance(type);
        }
    }
}