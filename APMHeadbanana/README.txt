ApmHeadbananaVolume
2024-2026 Haruka
Licensed under the GPLv3.

--------------------------------------------

Replacement DLL to make APM headphone controls customizable.

By default, replacing apmHeadphoneVolume.dll with no further configuration will make the headphone volume slider in Apmv3System and emoneyUI adjust the main left/right speaker volume.

For extended use, a client must be used such as APMHeadbananaLink (part of https://github.com/akechi-haruka/APMExtensions/)

--------------------------------------------

For developers:

Following functions are exported like the original dll:

float apmHeadphoneVolumeGet();
void apmHeadphoneVolumeSet(float val);

Following additional functions are provided:

int apmHeadbananaVersionGet();
- currently returns 3

void apmHeadphoneChannelsSet(const int* channels, const int len);
- Given an array of channels (max 8), this function will set the channels that apmHeadphoneVolumeSet will set, for example, if headphone channels are 2 and 3, call with apmHeadphoneChannelsSet({2,3}, 2);

void apmHeadphoneVolumeSetFullRange(bool full_range);
- The default range for the volume slider in APM is actually 0-50. Setting this to true will change the range to 0-100.

int apmHeadphoneChannelsGet();
- Returns a bitmask of currently set channels. For example, if headphone channels are 2 and 3, this will return 0b1100.

void apmHeadphoneChannelsSetInt(int channels);
- Set the channels that apmHeadphoneVolumeSet will set with a bitmask. For example, if the desired headphone channels are 2 and 3, pass 0b1100.
