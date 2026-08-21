using System;
using System.Diagnostics.CodeAnalysis;

namespace Haruka.Arcade.Apm.BananaphoneLib.MMDevice {
    [Flags]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    enum CLSCTX : uint {
        INPROC_SERVER = 1U,
        INPROC_HANDLER = 2U,
        LOCAL_SERVER = 4U,
        INPROC_SERVER16 = 8U,
        REMOTE_SERVER = 16U,
        INPROC_HANDLER16 = 32U,
        RESERVED1 = 64U,
        RESERVED2 = 128U,
        RESERVED3 = 256U,
        RESERVED4 = 512U,
        NO_CODE_DOWNLOAD = 1024U,
        RESERVED5 = 2048U,
        NO_CUSTOM_MARSHAL = 4096U,
        ENABLE_CODE_DOWNLOAD = 8192U,
        NO_FAILURE_LOG = 16384U,
        DISABLE_AAA = 32768U,
        ENABLE_AAA = 65536U,
        FROM_DEFAULT_CONTEXT = 131072U,
        INPROC = 3U,
        SERVER = 21U,
        ALL = 23U
    }
}