using MauiApp9.Popups;
using MauiAppCore.Services;
using UXDivers.Popups.Services;

namespace MauiApp9.Services;

public class CustomPopupService(IPopupService uxDiversPopupService) : ICustomPopupService
{
    public async Task ShowPopupAsync()
    {
        var popup = new UxDiversPopup();
        await uxDiversPopupService.PushAsync(popup);
    }

    public async Task ClosePopupAsync()
    {
        await uxDiversPopupService.PopAsync();
    }
}
