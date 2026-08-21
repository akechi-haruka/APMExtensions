using System.Runtime.InteropServices;

namespace Haruka.Arcade.Apm.BananaphoneLib.MMDevice {
    [Guid("2A07407E-6497-4A18-9787-32F79BD0D98F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IDeviceTopology {
        int NotImpl1();

        [PreserveSig]
        int GetConnector(uint nIndex, out IConnector connector);
    }
}