using CommunityToolkit.Maui.Views;
using Mopups.Pages;

namespace MauiApp9;

using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

public class BottomSheetPopup : PopupPage
{
    public BottomSheetPopup()
    {
        Background = Colors.Transparent; // Allow the background to be semi/transparent if needed

        // Define content
        Content = new Grid
        {
            BackgroundColor = Colors.CornflowerBlue,
            VerticalOptions = LayoutOptions.End, // Position at the bottom
            HorizontalOptions = LayoutOptions.Fill,
            HeightRequest = 700,
            Children =
            {
                new StackLayout
                {
                    Padding = 20,
                    Children =
                    {
                        new Label
                        {
                            Text = "This is an animated bottom sheet!",
                            FontSize = 20,
                            TextColor = Colors.White,
                        },
                        new Button
                        {
                            Text = "Close",
                            Command = new Command(ClosePopup),
                            BackgroundColor = Colors.White,
                            TextColor = Colors.Black,
                        },
                    },
                },
            },
        };

        // Set Animations
        this.Animation = new BottomSheetAnimation();
    }

    private void ClosePopup()
    {
        Mopups.Services.MopupService.Instance.PopAsync();
    }
}

// Define the custom bottom sheet animation
public class BottomSheetAnimation : Mopups.Animations.MoveAnimation
{
    // Override these if needed
    public BottomSheetAnimation()
    {
        DurationIn = 100; // Animation length for the “Appearing” action
        DurationOut = 300; // Animation length for the “Disappearing” action
    }

    public override void Preparing(View content, PopupPage page)
    {
        // Set durations here if the constructor isn't working:
        this.DurationIn = 100;
        this.DurationOut = 300;

        // Place the content below the visible area
        content.TranslationY = page.Height;
    }

    public override async Task Appearing(View content, PopupPage page)
    {
        // Wait briefly for layout to finish so content’s size is valid
        await content.TranslateTo(0, 0, DurationIn, Easing.SinIn);
    }

    public override async Task Disappearing(View content, PopupPage page)
    {
        await content.TranslateTo(0, page.Height, DurationOut, Easing.SinIn);
    }
}
