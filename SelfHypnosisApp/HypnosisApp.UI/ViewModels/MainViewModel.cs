using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HypnosisApp.Core.Models;
using HypnosisApp.UI.Data;
using System.Collections.ObjectModel;

namespace HypnosisApp.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ISessionRepository _repository;

    [ObservableProperty]
    private ObservableCollection<SessionTemplate> _sessions = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public MainViewModel(ISessionRepository repository)
    {
        _repository = repository;
    }

    [RelayCommand]
    private async Task LoadSessionsAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var sessions = await _repository.GetAllSessionsAsync();
            Sessions.Clear();
            foreach (var session in sessions)
            {
                Sessions.Add(session);
            }

            if (Sessions.Count == 0)
            {
                ErrorMessage = "No sessions available. Please add session templates.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading sessions: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task SelectSessionAsync(SessionTemplate session)
    {
        if (session == null) return;

        var navigationParameter = new Dictionary<string, object>
        {
            { "Session", session }
        };

        await Shell.Current.GoToAsync("SessionPlayerPage", navigationParameter);
    }

    [RelayCommand]
    private async Task ViewHistoryAsync()
    {
        await Shell.Current.GoToAsync("HistoryPage");
    }
}
