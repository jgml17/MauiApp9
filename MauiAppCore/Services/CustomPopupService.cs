namespace MauiAppCore.Services;

public class CustomPopupService : ICustomPopupService
{
    public async Task ShowPopupAsync(string message)
    {
        await Task.Delay(1);
    }
}
