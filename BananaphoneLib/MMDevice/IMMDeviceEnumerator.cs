using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Haruka.Arcade.Apm.BananaphoneLib.MMDevice {
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [ComImport]
    [ComVisible(true)]
    interface IMMDeviceEnumerator {
        int EnumAudioEndpoints(EDataFlow dataFlow, int stateMask,
            out IMMDeviceCollection devices);

        [PreserveSig]
        int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice endpoint);

        int GetDevice(string id, out IMMDevice deviceName);

        int RegisterEndpointNotificationCallback(IMMNotificationClient client);

        int UnregisterEndpointNotificationCallback(IMMNotificationClient client);
    }

    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    [ComImport]
    [ComVisible(true)]
    [ComSourceInterfaces(typeof(IMMDeviceEnumerator))]
    class MMDeviceEnumeratorComObject {
    }
}