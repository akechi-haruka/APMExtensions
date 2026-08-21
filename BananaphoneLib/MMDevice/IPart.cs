using System;
using System.Runtime.InteropServices;

namespace Haruka.Arcade.Apm.BananaphoneLib.MMDevice {
    [Guid("AE2DE0E4-5BCA-4F2D-AA46-5D13F8FDB3A9")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IPart {
        [PreserveSig]
        int GetName([MarshalAs(UnmanagedType.LPWStr)] out string name);

        int NotImpl1();

        int NotImpl2();

        int NotImpl3();

        [PreserveSig]
        int GetSubType(out Guid subType);

        int NotImpl4();

        int NotImpl5();

        [PreserveSig]
        int EnumPartsIncoming(out IPartsList parts);

        int NotImpl6();

        int NotImpl7();

        [PreserveSig]
        int Activate(CLSCTX clsContext, ref Guid refiid, [MarshalAs(UnmanagedType.IUnknown)] out object obj);
    }
}