using MauiApp9.Plugin.Contracts;

namespace MauiApp9.Plugin;

public class DefaultPluginPage : Contracts.IPluginPage
{
    public string PluginName => "Default MessagePage";
    public string Version => "1.0.0";

    public ContentPage CreatePage()
    {
        var page = new ContentPage { Title = "Default Message" };
        
        var stack = new VerticalStackLayout
        {
            Spacing = 25,
            Padding = new Thickness(30, 0),
            VerticalOptions = LayoutOptions.Center
        };

        stack.Children.Add(new Label
        {
            Text = "Default Plugin - No remote plugin loaded",
            FontSize = 18,
            HorizontalOptions = LayoutOptions.Center
        });

        page.Content = new ScrollView { Content = stack };
        return page;
    }
}