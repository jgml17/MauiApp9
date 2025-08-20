using Microsoft.Maui.Controls;

namespace MauiApp9;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("MainPage", typeof(MainPage));
        Routing.RegisterRoute("SingletonPage", typeof(SingletonPage));
        // Routing.RegisterRoute("OcrPage", typeof(OcrPage));
        Routing.RegisterRoute("OcrWithAiPage", typeof(OcrWithAiPage));

        Routing.RegisterRoute("ModalPage", typeof(ModalPage));
        Routing.RegisterRoute("StackPage", typeof(StackPage));
        // Routing.RegisterRoute("ModalPage//StackPage", typeof(StackPage));
    }
}
