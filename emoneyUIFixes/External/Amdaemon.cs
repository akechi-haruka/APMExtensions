using System;
using System.Runtime.InteropServices;

namespace Haruka.Arcade.EMUICF.External {
    public class Amdaemon {
        [DllImport("amdaemon_api")]
        public static extern void Core_execute();

        [DllImport("amdaemon_api")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool Core_isReady();

        [DllImport("amdaemon_api")]
        public static extern void Core_kill(NextProcess nextProcess);

        public static bool CheckDllWorking() {
            try {
                Core_execute();
                return Core_isReady();
            } catch (Exception ex) {
                Plugin.Log.LogWarning("Check for amdaemon failed: " + ex);
                return false;
            }
        }

        public enum NextProcess {
            Auto,
            SegaBoot,
            SegaBootError,
            SystemTest
        }
    }
}