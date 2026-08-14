using Haruka.Arcade.APMHeadbanana;

namespace Haruka.Arcade.EMUICF.External {
    public class BananaBridge {
        public const int MIN_VERSION = 3;

        public static bool IsWorking { get; private set; }

        public static void Check() {
            try {
                if (!Bananaphone.IsWorking) {
                    Plugin.Log.LogError("APMHeadbananaLink is reporting that apmHeadbanana is not working correctly.");
                    return;
                }

                int version = Native.ApmHeadbananaVersionGet();
                if (version < MIN_VERSION) {
                    Plugin.Log.LogError("apmHeadbanana is outdated, required is at least version " + MIN_VERSION + ", but " + version + " is present.");
                    return;
                }

                IsWorking = true;
            } catch {
                Plugin.Log.LogError("Bananaphone is not present!");
                IsWorking = false;
            } finally {
                if (!IsWorking) {
                    Plugin.Log.LogWarning("Advanced audio options are unavailable!");
                }
            }
        }
    }
}