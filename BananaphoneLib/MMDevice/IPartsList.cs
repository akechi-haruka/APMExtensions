using System.Runtime.InteropServices;

namespace Haruka.Arcade.Apm.BananaphoneLib.MMDevice {
    [Guid("6DAA848C-5EB0-45CC-AEA5-998A2CDA1FFB")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IPartsList {
        [PreserveSig]
        int GetCount(out uint count);

        [PreserveSig]
        int GetPart(uint nIndex, out IPart part);
    }
}