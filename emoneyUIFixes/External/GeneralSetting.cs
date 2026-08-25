using System;

namespace Haruka.Arcade.Apm.EMUICF.External {
    [Serializable]
    public class GeneralSetting {
        public LedSetting ledSetting;
    }

    [Serializable]
    public class LedSetting {
        public int portNumber;
    }
}