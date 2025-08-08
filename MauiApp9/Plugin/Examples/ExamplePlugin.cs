// This is an example of what your separate plugin DLL would contain
// You would compile this as a separate project/DLL

/*
using MauiApp9.Plugin;

namespace MauiApp9.Plugins;

public class CustomMessagePlugin : IPluginPage
{
    public string PluginName => "Custom Message Plugin";
    public string Version => "2.1.0";

    public ContentPage CreatePage()
    {
        var page = new ContentPage 
        { 
            Title = "Custom Plugin Page",
            BackgroundColor = Colors.LightBlue
        };
        
        var stack = new VerticalStackLayout
        {
            Spacing = 30,
            Padding = new Thickness(40, 50),
            VerticalOptions = LayoutOptions.Center
        };

        var titleLabel = new Label
        {
            Text = "🚀 Custom Plugin Loaded!",
            FontSize = 24,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.DarkBlue
        };

        var versionLabel = new Label
        {
            Text = $"Plugin Version: {Version}",
            FontSize = 14,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.Gray
        };

        var actionButton = new Button
        {
            Text = "Execute Plugin Action",
            BackgroundColor = Colors.Green,
            TextColor = Colors.White,
            CornerRadius = 25,
            HeightRequest = 50
        };

        var resultLabel = new Label
        {
            Text = "",
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center,
            IsVisible = false,
            TextColor = Colors.DarkGreen
        };

        actionButton.Clicked += async (s, e) =>
        {
            resultLabel.Text = "✅ Plugin action executed successfully!";
            resultLabel.IsVisible = true;
            
            await page.DisplayAlert(
                "Plugin Action", 
                "This alert was triggered by the downloaded plugin DLL!", 
                "Amazing!"
            );
        };

        stack.Children.Add(titleLabel);
        stack.Children.Add(versionLabel);
        stack.Children.Add(actionButton);
        stack.Children.Add(resultLabel);

        page.Content = new ScrollView { Content = stack };
        return page;
    }
}
*/