using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Haruka.Arcade.Apm.EMUICF {
    [UsedImplicitly]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnassignedField.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")]
    [Serializable]
    public class AppExConfig {
        public int version;
        public Exit exit;
        public LedSettings led;

        [Serializable]
        public class Exit {
            public bool kill;
            public String[] kill_process_name_list;
        }

        [Serializable]
        public class LedSettings {
            public bool block_user_change;
            public bool ignore;
            public byte r;
            public byte g;
            public byte b;
        }

        [Serializable]
        public class GuideInfo {
            public int width = 800;
            public int height = 400;
            public List<GuidePage> pages = new List<GuidePage>();
        }

        [Serializable]
        public class GuidePage {
            public String title;
            public String text;
            public String file;
            public String align;
            public GuideContent content = new GuideContent();

            public override string ToString() {
                return title;
            }
        }

        [Serializable]
        public class GuideContent {
            public String align = "center";
            public List<GuideButton> buttons = new List<GuideButton>();
        }

        public class GuideButton {
            public int x;
            public int y;
            public bool center;
            public int width;
            public int height;
            public String text;
            public int target;

            public override string ToString() {
                return "Button: " + text;
            }
        }
    }
}