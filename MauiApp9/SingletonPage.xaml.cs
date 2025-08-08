using System;
using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;

namespace MauiApp9;

public partial class SingletonPage : ContentPage
{
    public SingletonPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    private async void Button_OnClicked(object? sender, EventArgs e)
    {
        var bottomSheet = new BottomSheetPopup();
        await Mopups.Services.MopupService.Instance.PushAsync(bottomSheet);
    }

    private async void Button_Popup_OnClicked(object? sender, EventArgs e)
    {
        popup.Show();
    }

    private void Button_Sync_OnClicked(object? sender, EventArgs e)
    {
        if (bottomSheet.IsOpen)
            bottomSheet.Close();
        else
            bottomSheet.IsOpen = true;
        //bottomSheet.Show();
    }

    private void Button_CloseBottomSheet_OnClicked(object? sender, EventArgs e)
    {
        bottomSheet.IsOpen = false;
        // bottomSheet.Close();
    }
}
