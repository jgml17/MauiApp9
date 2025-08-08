using MauiApp9.Plugin.Contracts;

namespace MauiApp9.Plugin;

public class PluginMessagePage : ContentPage
{
    private Contracts.IPluginPage _currentPlugin;
    private ContentPage _pluginPage;

    public PluginMessagePage()
    {
        Title = "Plugin Message";
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await LoadPluginAsync();
        CreateUI();
    }

    private async Task LoadPluginAsync()
    {
        _currentPlugin = await AssemblyPluginManager.LoadPluginAsync();
        _pluginPage = _currentPlugin.CreatePage();
    }

    private void CreateUI()
    {
        var mainStack = new VerticalStackLayout { Spacing = 10, Padding = 20 };

        // Plugin info header
        var headerStack = new HorizontalStackLayout { Spacing = 10 };

        var pluginLabel = new Label
        {
            Text = $"{_currentPlugin.PluginName} v{_currentPlugin.Version}",
            FontSize = 12,
            VerticalOptions = LayoutOptions.Center,
        };

        var updateButton = new Button
        {
            Text = "Check Updates",
            FontSize = 12,
            HorizontalOptions = LayoutOptions.End,
            BackgroundColor = Colors.Orange,
        };

        updateButton.Clicked += OnCheckUpdatesClicked;

        headerStack.Children.Add(pluginLabel);
        headerStack.Children.Add(updateButton);

        // Add plugin content
        mainStack.Children.Add(headerStack);
        mainStack.Children.Add(new BoxView { HeightRequest = 1, BackgroundColor = Colors.Gray });
        mainStack.Children.Add(_pluginPage.Content);

        Content = new ScrollView { Content = mainStack };
    }

    private async void OnCheckUpdatesClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        button.Text = "Checking...";
        button.IsEnabled = false;

        try
        {
            // const string apiUrl = "http://10.0.2.2:5000";
            const string apiUrl = "http://192.168.68.57:5001";
            // const string apiUrl = "http://localhost:5000";

            var remoteInfo = await AssemblyPluginManager.GetRemotePluginInfoAsync(apiUrl);
            var localInfo = await AssemblyPluginManager.GetLocalPluginInfoAsync();

            if (AssemblyPluginManager.HasUpdate(localInfo, remoteInfo))
            {
                var download = await DisplayAlert(
                    "Update Available",
                    $"Plugin '{remoteInfo.Name}' v{remoteInfo.Version} is available.\nDownload now?",
                    "Yes",
                    "No"
                );

                if (download)
                {
                    button.Text = "Downloading...";
                    var success = await AssemblyPluginManager.DownloadPluginAsync(remoteInfo);

                    if (success)
                    {
                        button.Text = "Reloading...";
                        await LoadPluginAsync();
                        CreateUI();
                        await DisplayAlert(
                            "Success",
                            $"Plugin updated to v{_currentPlugin.Version}!",
                            "OK"
                        );
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to download plugin.", "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("No Updates", "You have the latest plugin version.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Update check failed: {ex.Message}", "OK");
        }
        finally
        {
            button.Text = "Check Updates";
            button.IsEnabled = true;
        }
    }
}
