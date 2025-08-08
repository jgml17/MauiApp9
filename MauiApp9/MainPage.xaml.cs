using System;
using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;

namespace MauiApp9;

public partial class MainPage
{
    // public string TabName { get; set; }
    
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
        Shell.Current.Navigating += CurrentOnNavigating;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    private void CurrentOnNavigating(object? sender, ShellNavigatingEventArgs e)
    {
        if (e.Source == ShellNavigationSource.Pop) return;
        if (e.Target.Location.OriginalString.Contains("MainPage"))
        {
            var query = e.Target.Location.OriginalString;
            CurrentTab.Text = query.Contains("vendas") ? "Vendas" : "Pedidos";
        }
    }

    private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"ModalPage");
    }
}