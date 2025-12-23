using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppCore.Services;

namespace MauiAppCore;

public partial class UxdiversViewModel(
    ICustomPopupService popupService,
    INavigationParametersService navigationParametersService
) : ObservableObject
{
    [ObservableProperty]
    public partial string Status { get; set; } = "Here we will test UxDivers Popup";

    [RelayCommand]
    private async Task ShowPopup()
    {
        navigationParametersService.ParameterValue = Status;
        await popupService.ShowPopupAsync();
    }
}
