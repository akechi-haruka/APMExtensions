using System.Runtime.InteropServices;

namespace Haruka.Arcade.Apm.BananaphoneLib.MMDevice {
    [Guid("9c2c4058-23f5-41de-877a-df3af236a09e")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IConnector {
        int NotImpl1();

        int NotImpl2();

        int NotImpl3();

        int NotImpl4();

        int NotImpl5();

        [PreserveSig]
        int GetConnectedTo(out IConnector connectTo);
    }
}