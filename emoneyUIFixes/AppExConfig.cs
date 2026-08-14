using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Haruka.Arcade.EMUICF {
    [UsedImplicitly]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnassignedField.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")]
    [Serializable]
    public struct AppExConfig {
        public int version;
        public Exit exit;
        public GuideInfo guide;

        [Serializable]
        public struct Exit {
            public bool kill;
            public String[] kill_process_name_list;
        }

        [Serializable]
        public struct GuideInfo {
            public int width;
            public int height;
            public GuidePage[] pages;
        }

        [Serializable]
        public struct GuidePage {
            public String title;
            public String text;
            public String file;
            public String align;
            public GuideContent content;
        }

        [Serializable]
        public struct GuideContent {
            public String align;
            public GuideButton[] buttons;
        }

        public struct GuideButton {
            public int x;
            public int y;
            public bool center;
            public int width;
            public int height;
            public String text;
            public int target;
        }
    }
}