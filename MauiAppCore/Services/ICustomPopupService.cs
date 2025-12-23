namespace MauiAppCore.Services;

public interface ICustomPopupService
{
    Task ShowPopupAsync();
    Task ClosePopupAsync();
}
