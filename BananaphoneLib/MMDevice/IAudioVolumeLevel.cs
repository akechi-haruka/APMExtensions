using System.Runtime.InteropServices;

namespace Haruka.Arcade.Apm.BananaphoneLib.MMDevice {
    [Guid("7FB7B48F-531D-44A2-BCB3-5AD5A134B3DC")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioVolumeLevel : IPerChannelDbLevel {
    }
}