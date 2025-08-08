using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp9;

public partial class StackPage : ContentPage
{
    public StackPage()
    {
        InitializeComponent();
    }

    private void Button_OnRootClicked(object? sender, EventArgs e)
    {
        Shell.Current.Navigation.PopToRootAsync();
    }

    private void Button_OnBackClicked(object? sender, EventArgs e)
    {
        Shell.Current.Navigation.PopModalAsync();
    }
}