using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using MauiAppCore;

namespace MauiApp9;

public partial class UxdiversPage : ContentPage
{
    private readonly UxdiversViewModel _viewModel;

    public UxdiversPage(UxdiversViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    private void TouchBehavior_OnTouchGestureCompleted(
        object? sender,
        TouchGestureCompletedEventArgs e
    )
    {
        _viewModel.Status = $"Touch gesture completed";
    }
}
