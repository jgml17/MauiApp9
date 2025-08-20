using CommunityToolkit.Maui;
using MemoryToolkit.Maui;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Plugin.Maui.OCR;
using Syncfusion.Maui.Toolkit.Hosting;

namespace MauiApp9;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureMopups()
            .ConfigureSyncfusionToolkit()
            // .UseOcr()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<BottomSheetPopup>();

        // builder.Services.AddSingleton(OcrPlugin.Default);

#if DEBUG
        builder.Logging.AddDebug();
        builder.UseLeakDetection();
#endif

        return builder.Build();
    }
}
