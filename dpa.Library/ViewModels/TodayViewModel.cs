using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using dpa.Library.Models;
using dpa.Library.Services;

namespace dpa.Library.ViewModels;

public class TodayViewModel : ViewModelBase
{   private readonly ITodayPoetryService _todayPoetryService;

    public TodayViewModel(ITodayPoetryService todayPoetryService)
    {
        _todayPoetryService = todayPoetryService;
        OnInitializedCommand = new AsyncRelayCommand(OnInitializedAsync);
    } 
    
    private TodayPoetry _todayPoetry;

    public TodayPoetry TodayPoetry {
        get => _todayPoetry;
        set => SetProperty(ref _todayPoetry, value);
    }
    
    private bool _isLoading;

    public bool IsLoading {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }
    
    public ICommand OnInitializedCommand { get; }

    public async Task OnInitializedAsync()
    {
        IsLoading = true;
        TodayPoetry = await _todayPoetryService.GetTodayPoetryAsync();
        IsLoading = false;
    }
}