using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace MauiApp9;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Sets App Theme Dark or Light
        if (Current != null)
            Current.UserAppTheme = AppTheme.Light;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
