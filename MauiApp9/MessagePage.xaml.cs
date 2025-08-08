namespace MauiApp9;

public partial class MessagePage : ContentPage
{
    public MessagePage()
    {
        InitializeComponent();
    }

    private async void OnShowMessageClicked(object sender, EventArgs e)
    {
        MessageLabel.Text = "Hello! This is your message from the button click!";
        MessageLabel.IsVisible = true;
        
        await DisplayAlert("Message", "Button clicked successfully!", "OK");
    }
}