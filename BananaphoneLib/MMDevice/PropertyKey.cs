using System;
using System.Diagnostics.CodeAnalysis;

namespace Haruka.Arcade.Apm.BananaphoneLib.MMDevice {
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct PropertyKey {
        public Guid formatId;
        public int propertyId;

        public PropertyKey(Guid formatId, int propertyId) {
            this.formatId = formatId;
            this.propertyId = propertyId;
        }
    }
}