using System;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using FH6Mod.Services;
using FH6Mod.ViewModels.Pages;
using Material.Icons;
using Microsoft.Extensions.DependencyInjection;

namespace FH6Mod.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly GameProcessService _gameProcess;
    private readonly UpdateCheckService _updater;

    public ObservableCollection<NavItem> NavItems { get; }
    public ObservableCollection<NavItem> FooterNavItems { get; }

    [ObservableProperty] private NavItem? _mainSelectedItem;
    [ObservableProperty] private NavItem? _footerSelectedItem;
    private bool _syncingNav;

    [ObservableProperty]
    private ViewModelBase? _currentPage;

    [ObservableProperty]
    private string _gameStatusText = "FH6 disconnected";

    [ObservableProperty]
    private string _gameStatusDetail = "Launch the game and the trainer will attach automatically.";

    [ObservableProperty]
    private bool _isGameAttached;

    [ObservableProperty]
    private bool _isUpdateAvailable;

    [ObservableProperty]
    private string _updateChipText = "";

    [ObservableProperty]
    private string _updateTooltip = "";

    [ObservableProperty]
    private string _updateFooterText = "Checking for updates…";

    [ObservableProperty]
    private UpdateFooterStatus _updateFooterStatus = UpdateFooterStatus.Checking;

    public string ReleasesUrl => UpdateCheckService.ReleasesUrl;

    public string CurrentVersionText => $"v{App.Services.GetRequiredService<UpdateCheckService>().CurrentVersion.ToString(3)}";

    public MainWindowViewModel()
        : this(
            App.Services.GetRequiredService<GameProcessService>(),
            App.Services.GetRequiredService<UpdateCheckService>())
    {
    }

    public MainWindowViewModel(GameProcessService gameProcess, UpdateCheckService updater)
    {
        _gameProcess = gameProcess;
        _updater = updater;
        _updater.StateChanged += OnUpdateStateChanged;
        _updater.CheckInBackground();

        NavItems = new ObservableCollection<NavItem>
        {
            new("Dashboard",      MaterialIconKind.ViewDashboardOutline,        typeof(DashboardViewModel),    IsWorking: true),
            new("Unlocks",        MaterialIconKind.LockOpenVariantOutline,      typeof(UnlocksViewModel),      IsWorking: true),
            new("Database",       MaterialIconKind.DatabaseEditOutline,         typeof(DatabaseViewModel),     IsWorking: true),
        };

        FooterNavItems = new ObservableCollection<NavItem>
        {
            new("Settings",       MaterialIconKind.CogOutline,                  typeof(SettingsViewModel),     IsWorking: true),
        };

        MainSelectedItem = NavItems[0];

        _gameProcess.StatusChanged += OnGameStatusChanged;
        OnGameStatusChanged();
    }

    partial void OnMainSelectedItemChanged(NavItem? value)
    {
        if (_syncingNav || value is null) return;
        _syncingNav = true;
        try
        {
            FooterSelectedItem = null;
            CurrentPage = (ViewModelBase)App.Services.GetRequiredService(value.PageType);
        }
        finally { _syncingNav = false; }
    }

    partial void OnFooterSelectedItemChanged(NavItem? value)
    {
        if (_syncingNav || value is null) return;
        _syncingNav = true;
        try
        {
            MainSelectedItem = null;
            CurrentPage = (ViewModelBase)App.Services.GetRequiredService(value.PageType);
        }
        finally { _syncingNav = false; }
    }

    private void OnGameStatusChanged()
    {
        Dispatcher.UIThread.Post(() =>
        {
            IsGameAttached = _gameProcess.IsAttached;
            if (_gameProcess.IsAttached)
            {
                GameStatusText = $"FH6 connected · PID {_gameProcess.Pid}";
                GameStatusDetail = $"Base 0x{_gameProcess.BaseAddress.ToInt64():X} · {_gameProcess.ModuleSize / 1024 / 1024} MB module";
            }
            else
            {
                GameStatusText = "FH6 disconnected";
                GameStatusDetail = "Launch the game and the trainer will attach automatically.";
            }
        });
    }

    private void OnUpdateStateChanged()
    {
        if (_updater.LastError != null)
        {
            UpdateFooterText = "Update check failed (no internet?)";
            UpdateFooterStatus = UpdateFooterStatus.Failed;
            return;
        }

        if (_updater.IsUpdateAvailable)
        {
            IsUpdateAvailable = true;
            UpdateChipText = $"{_updater.LatestTag} available";
            UpdateTooltip  = $"New version {_updater.LatestTag} is available (you have v{_updater.CurrentVersion.ToString(3)}). Click to open the releases page.";
            UpdateFooterText = $"Update available · {_updater.LatestTag}";
            UpdateFooterStatus = UpdateFooterStatus.UpdateAvailable;
        }
        else
        {
            UpdateFooterText = $"Up to date · v{_updater.CurrentVersion.ToString(3)}";
            UpdateFooterStatus = UpdateFooterStatus.UpToDate;
        }
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private void OpenReleasesPage()
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = UpdateCheckService.ReleasesUrl,
                UseShellExecute = true,
            });
        }
        catch { }
    }
}

public sealed record NavItem(string Label, MaterialIconKind Icon, Type PageType, bool IsWorking = false);

public enum UpdateFooterStatus { Checking, UpToDate, UpdateAvailable, Failed }
