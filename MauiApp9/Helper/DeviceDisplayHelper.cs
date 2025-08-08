namespace MauiApp9.Helper;

public static class DeviceDisplayHelper
{
    public static double ScreenHeight =>
        DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
    public static double ScreenWidth =>
        DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

    public static double ContentHeight => ScreenHeight - TabBarHeight;

    private static double TabBarHeight =>
#if IOS
        83; // iOS tab bar height
#elif ANDROID
        56; // Android bottom navigation height
#else
        56; // Default fallback
#endif
}
