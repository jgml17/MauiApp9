using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp9;

public partial class ModalPage : ContentPage, IDisposable
{
    public ModalPage()
    {
        InitializeComponent();
    }

    private void Exit_OnClicked(object? sender, EventArgs e)
    {
        Shell.Current.Navigation.PopToRootAsync();
        // Shell.Current.GoToAsync($"..");
    }

    private void OpenStack_OnClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"StackPage");
        // Shell.Current.Navigation.PushModalAsync(new StackPage());
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}