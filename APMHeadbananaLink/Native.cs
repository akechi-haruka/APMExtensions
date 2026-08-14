using System.Runtime.InteropServices;

namespace Haruka.Arcade.APMHeadbanana {
    public class Native {
        [DllImport("apmHeadphoneVolume", EntryPoint = "apmHeadbananaVersionGet")]
        public static extern int ApmHeadbananaVersionGet();

        [DllImport("apmHeadphoneVolume", EntryPoint = "apmHeadphoneVolumeGet")]
        public static extern float ApmHeadphoneVolumeGet();

        [DllImport("apmHeadphoneVolume", EntryPoint = "apmHeadphoneVolumeSet")]
        public static extern void ApmHeadphoneVolumeSet(float volume);

        [DllImport("apmHeadphoneVolume", EntryPoint = "apmHeadphoneChannelsSet")]
        public static extern void ApmHeadphoneChannelsSet([MarshalAs(UnmanagedType.LPArray)] int[] channels, int len);

        [DllImport("apmHeadphoneVolume", EntryPoint = "apmHeadphoneChannelsGet")]
        public static extern int ApmHeadphoneChannelsGet();

        [DllImport("apmHeadphoneVolume", EntryPoint = "apmHeadphoneChannelsSetInt")]
        public static extern void ApmHeadphoneChannelsSetInt(int channels);

        [DllImport("apmHeadphoneVolume", EntryPoint = "apmHeadphoneVolumeSetFullRange")]
        public static extern void ApmHeadphoneVolumeSetFullRange(bool fullRange);
    }
}