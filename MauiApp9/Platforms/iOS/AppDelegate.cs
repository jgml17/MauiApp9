using Foundation;
using UIKit;

namespace MauiApp9;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        // Add any additional setup after launching the app
        new ImageCropper.Maui.Platform().Init();

        return base.FinishedLaunching(app, options);
    }
}
