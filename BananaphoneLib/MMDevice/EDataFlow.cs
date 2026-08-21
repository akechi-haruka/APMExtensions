using System.Diagnostics.CodeAnalysis;

namespace Haruka.Arcade.Apm.BananaphoneLib.MMDevice {
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    enum EDataFlow {
        ERender,
        ECapture,
        EAll,
        EDataFlow_enum_count
    }
}