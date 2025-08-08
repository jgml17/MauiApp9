using System.Collections.ObjectModel;
using MauiApp9.Plugin.Contracts;

namespace MauiApp9.Plugins;

public class CustomMessagePlugin : IPluginPage
{
    public string PluginName => "Advanced Message Plugin";
    public string Version => "2.0.0";

    private ObservableCollection<MessageItem> _messages = new();
    private int _messageCounter = 1;

    public ContentPage CreatePage()
    {
        var page = new ContentPage { Title = "Advanced Plugin" };

        // Create gradient background
        var gradientBrush = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1),
        };
        gradientBrush.GradientStops.Add(
            new GradientStop { Color = Color.FromArgb("#667eea"), Offset = 0.0f }
        );
        gradientBrush.GradientStops.Add(
            new GradientStop { Color = Color.FromArgb("#764ba2"), Offset = 1.0f }
        );
        page.Background = gradientBrush;

        var mainGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Auto },
            },
            Padding = 20,
            RowSpacing = 15,
        };

        // Header section
        var headerFrame = new Frame
        {
            BackgroundColor = Colors.White,
            CornerRadius = 15,
            HasShadow = true,
            Padding = 20,
        };

        var headerStack = new VerticalStackLayout { Spacing = 10 };

        var titleLabel = new Label
        {
            Text = "🎯 Advanced Plugin Dashboard",
            FontSize = 22,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Color.FromArgb("#2c3e50"),
        };

        var versionBadge = new Frame
        {
            BackgroundColor = Color.FromArgb("#3498db"),
            CornerRadius = 12,
            Padding = new Thickness(12, 6),
            HorizontalOptions = LayoutOptions.Center,
        };

        var versionLabel = new Label
        {
            Text = $"v{Version}",
            FontSize = 12,
            TextColor = Colors.White,
            FontAttributes = FontAttributes.Bold,
        };

        versionBadge.Content = versionLabel;
        headerStack.Children.Add(titleLabel);
        headerStack.Children.Add(versionBadge);
        headerFrame.Content = headerStack;

        // Stats section
        var statsGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
            },
            ColumnSpacing = 10,
        };

        var messagesCard = CreateStatCard("📨", "Messages", "0", "#e74c3c");
        var actionsCard = CreateStatCard("⚡", "Actions", "0", "#f39c12");
        var statusCard = CreateStatCard("✅", "Status", "Active", "#27ae60");

        statsGrid.Add(messagesCard, 0, 0);
        statsGrid.Add(actionsCard, 1, 0);
        statsGrid.Add(statusCard, 2, 0);

        // Messages list
        var messagesFrame = new Frame
        {
            BackgroundColor = Colors.White,
            CornerRadius = 15,
            HasShadow = true,
            Padding = 15,
        };

        var messagesStack = new VerticalStackLayout { Spacing = 10 };

        var messagesTitle = new Label
        {
            Text = "📋 Recent Messages",
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#2c3e50"),
        };

        var messagesList = new CollectionView
        {
            ItemsSource = _messages,
            HeightRequest = 200,
            ItemTemplate = new DataTemplate(() =>
            {
                var grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Auto },
                    },
                    Padding = 10,
                    ColumnSpacing = 10,
                };

                var iconLabel = new Label { FontSize = 20, VerticalOptions = LayoutOptions.Center };
                iconLabel.SetBinding(Label.TextProperty, "Icon");

                var textLabel = new Label
                {
                    FontSize = 14,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.FromArgb("#34495e"),
                };
                textLabel.SetBinding(Label.TextProperty, "Text");

                var timeLabel = new Label
                {
                    FontSize = 12,
                    TextColor = Colors.Gray,
                    VerticalOptions = LayoutOptions.Center,
                };
                timeLabel.SetBinding(Label.TextProperty, "Time");

                grid.Add(iconLabel, 0, 0);
                grid.Add(textLabel, 1, 0);
                grid.Add(timeLabel, 2, 0);

                return new Frame
                {
                    BackgroundColor = Color.FromArgb("#f8f9fa"),
                    CornerRadius = 8,
                    Padding = 0,
                    Margin = new Thickness(0, 2),
                    Content = grid,
                };
            }),
        };

        messagesStack.Children.Add(messagesTitle);
        messagesStack.Children.Add(messagesList);
        messagesFrame.Content = messagesStack;

        // Action buttons
        var buttonsGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
            },
            ColumnSpacing = 10,
        };

        var addMessageBtn = new Button
        {
            Text = "➕ Add Message",
            BackgroundColor = Color.FromArgb("#3498db"),
            TextColor = Colors.White,
            CornerRadius = 25,
            HeightRequest = 50,
            FontAttributes = FontAttributes.Bold,
        };

        var clearBtn = new Button
        {
            Text = "🗑️ Clear All",
            BackgroundColor = Color.FromArgb("#e74c3c"),
            TextColor = Colors.White,
            CornerRadius = 25,
            HeightRequest = 50,
            FontAttributes = FontAttributes.Bold,
        };

        addMessageBtn.Clicked += async (s, e) =>
        {
            _messages.Add(
                new MessageItem
                {
                    Icon = "💬",
                    Text = $"Message #{_messageCounter++}",
                    Time = DateTime.Now.ToString("HH:mm"),
                }
            );

            UpdateStatCard(messagesCard, _messages.Count.ToString());

            await page.DisplayAlert("Success", "Message added to the list!", "OK");
        };

        clearBtn.Clicked += async (s, e) =>
        {
            var confirm = await page.DisplayAlert("Confirm", "Clear all messages?", "Yes", "No");
            if (confirm)
            {
                _messages.Clear();
                _messageCounter = 1;
                UpdateStatCard(messagesCard, "0");
            }
        };

        buttonsGrid.Add(addMessageBtn, 0, 0);
        buttonsGrid.Add(clearBtn, 1, 0);

        mainGrid.Add(headerFrame, 0, 0);
        mainGrid.Add(statsGrid, 0, 1);
        mainGrid.Add(messagesFrame, 0, 2);
        mainGrid.Add(buttonsGrid, 0, 3);

        page.Content = new ScrollView { Content = mainGrid };
        return page;
    }

    private Frame CreateStatCard(string icon, string title, string value, string color)
    {
        var frame = new Frame
        {
            BackgroundColor = Colors.White,
            CornerRadius = 12,
            HasShadow = true,
            Padding = 15,
        };

        var stack = new VerticalStackLayout
        {
            Spacing = 8,
            HorizontalOptions = LayoutOptions.Center,
        };

        var iconLabel = new Label
        {
            Text = icon,
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center,
        };

        var titleLabel = new Label
        {
            Text = title,
            FontSize = 12,
            TextColor = Colors.Gray,
            HorizontalOptions = LayoutOptions.Center,
        };

        var valueLabel = new Label
        {
            Text = value,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb(color),
            HorizontalOptions = LayoutOptions.Center,
        };

        stack.Children.Add(iconLabel);
        stack.Children.Add(titleLabel);
        stack.Children.Add(valueLabel);

        frame.Content = stack;
        return frame;
    }

    private void UpdateStatCard(Frame card, string newValue)
    {
        var stack = (VerticalStackLayout)card.Content;
        var valueLabel = (Label)stack.Children[2];
        valueLabel.Text = newValue;
    }
}

public class MessageItem
{
    public string Icon { get; set; } = "";
    public string Text { get; set; } = "";
    public string Time { get; set; } = "";
}
