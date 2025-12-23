using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MauiAppCore.Services;
using UXDivers.Popups.Maui.Controls;
using UXDivers.Popups.Services;

namespace MauiApp9.Popups;

public partial class UxDiversPopup : ActionModalPopup
{
    private static readonly BindableProperty EntryProperty = BindableProperty.Create(
        nameof(Entry),
        typeof(string),
        typeof(UxDiversPopup),
        string.Empty
    );

    public string Entry
    {
        get => (string)GetValue(EntryProperty);
        set => SetValue(EntryProperty, value);
    }

    public UxDiversPopup()
    {
        InitializeComponent();
        var service = Application
            .Current!.Windows[0]
            .Page?.Handler?.MauiContext?.Services.GetService<INavigationParametersService>();
        Entry = service?.ParameterValue ?? string.Empty;

        ActionButtonCommand = new Command(() =>
        {
            // Your custom logic here
            service?.ParameterValue = Entry;
            IPopupService.Current.PopAsync();
        });
    }
}
