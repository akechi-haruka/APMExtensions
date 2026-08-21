using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Haruka.Arcade.Apm.BananaphoneLib.MMDevice {
    [Guid("C2F8E001-F205-4BC9-99BC-C13B1E048CCB")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    interface IPerChannelDbLevel {
        int NotImpl1();

        [PreserveSig]
        int GetLevelRange(uint channel, out float minLevelDB, out float maxLevelDB, out float stepping);

        [PreserveSig]
        int GetLevel(uint nChannel, out float levelDB);

        [PreserveSig]
        int SetLevel(uint nChannel, float levelDB, Guid guidEventContext);
    }
}