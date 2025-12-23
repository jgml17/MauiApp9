using CommunityToolkit.Maui;
using MauiApp9.Services;
using MauiAppCore;
using MauiAppCore.Services;
using MemoryToolkit.Maui;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Plugin.Maui.OCR;
using Syncfusion.Maui.Toolkit.Hosting;
using UXDivers.Popups.Maui;

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
            .UseUXDiversPopups()
            .ConfigureSyncfusionToolkit()
            .UseOcr()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<BottomSheetPopup>();

        // Register services
        builder.Services.AddSingleton<ICustomPopupService, CustomPopupService>();
        builder.Services.AddSingleton<INavigationParametersService, NavigationParametersService>();

        // Register ViewModels
        builder.Services.AddTransient<UxdiversViewModel>();

        builder.Services.AddSingleton(OcrPlugin.Default);
        builder.Logging.AddDebug();
        builder.UseLeakDetection();

        return builder.Build();
    }
}
