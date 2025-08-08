using MauiApp9.Plugin.Contracts;

namespace ChristmasMessagePlugin;

public class ChristmasMessagePlugin : IPluginPage
{
    public string PluginName => "Christmas Message Center";
    public string Version => "5.0.0";

    public ContentPage CreatePage()
    {
        return new ChristmasPage();
    }
}