using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppCore.Services;

namespace MauiAppCore;

public partial class UxdiversViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Status { get; set; } = "Here we will test UxDivers Popup";
}
