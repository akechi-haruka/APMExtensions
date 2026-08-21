using System;
using System.Runtime.InteropServices;

namespace Haruka.Arcade.Apm.BananaphoneLib.MMDevice {
    [Guid("5CDF2C82-841E-4546-9722-0CF74078229A")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioEndpointVolume {
        int NotImpl1();

        int NotImpl2();

        [PreserveSig]
        int GetChannelCount(out int pnChannelCount);

        int NotImpl3();

        int NotImpl4();

        int NotImpl5();

        int NotImpl6();

        int NotImpl7();

        [PreserveSig]
        int SetChannelVolumeLevelScalar(uint nChannel, float fLevel, Guid pguidEventContext);

        int NotImpl8();

        [PreserveSig]
        int GetChannelVolumeLevelScalar(uint nChannel, out float pfLevel);
    }
}