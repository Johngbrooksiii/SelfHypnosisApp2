using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HypnosisApp.Core.Models;
using HypnosisApp.Core.Services;
using HypnosisApp.UI.Data;

namespace HypnosisApp.UI.ViewModels;

[QueryProperty(nameof(Session), "Session")]
public partial class SessionPlayerViewModel : ObservableObject
{
    private readonly ISessionPlayer _player;
    private readonly ISessionRepository _repository;
    private CancellationTokenSource? _cancellationTokenSource;
    private DateTime _sessionStartTime;

    [ObservableProperty]
    private SessionTemplate? _session;

    [ObservableProperty]
    private string _currentStage = string.Empty;

    [ObservableProperty]
    private double _progress;

    [ObservableProperty]
    private bool _isPlaying;

    [ObservableProperty]
    private string _statusMessage = "Ready to begin";

    public SessionPlayerViewModel(ISessionPlayer player, ISessionRepository repository)
    {
        _player = player;
        _repository = repository;
    }

    [RelayCommand]
    private async Task StartSessionAsync()
    {
        if (Session == null || IsPlaying) return;

        try
        {
            IsPlaying = true;
            StatusMessage = "Starting session...";
            _sessionStartTime = DateTime.Now;
            _cancellationTokenSource = new CancellationTokenSource();

            await _player.PlaySessionAsync(Session);

            // Log successful completion
            await _repository.LogSessionAsync(
                Session.Title,
                _sessionStartTime,
                DateTime.Now,
                completed: true);

            StatusMessage = "Session completed successfully";
            IsPlaying = false;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
            IsPlaying = false;

            // Log incomplete session
            await _repository.LogSessionAsync(
                Session?.Title ?? "Unknown",
                _sessionStartTime,
                DateTime.Now,
                completed: false);
        }
    }

    [RelayCommand]
    private void StopSession()
    {
        _player.StopSession();
        _cancellationTokenSource?.Cancel();
        IsPlaying = false;
        StatusMessage = "Session stopped";
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        if (IsPlaying)
        {
            StopSession();
        }
        await Shell.Current.GoToAsync("..");
    }
}
