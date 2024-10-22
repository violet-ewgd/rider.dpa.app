using System;
using System.Net;
using Avalonia;
using dpa.Library.Services;
using dpa.Library.ViewModels;
using dpa.Services;
using Microsoft.Extensions.DependencyInjection;

namespace dpa;

public class ServiceLocator {
    private readonly IServiceProvider _serviceProvider;

    private static ServiceLocator _current;

    public static ServiceLocator Current {
        get
        {
            if (_current is not null) {
                return _current;
            }

            if (Application.Current.TryGetResource(nameof(ServiceLocator),
                    null, out var resource) &&
                resource is ServiceLocator serviceLocator) {
                return _current = serviceLocator;
            }

            throw new Exception("理论上来讲不应该发生这种情况。");
        }
    }
    
    public ResultViewModel ResultViewModel => 
        _serviceProvider.GetRequiredService<ResultViewModel>();

    public TodayViewModel TodayViewModel =>
        _serviceProvider.GetRequiredService<TodayViewModel>();
    public ServiceLocator() {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<IPreferenceStorage, FilePreferenceStorage>();
        serviceCollection.AddSingleton<IPoetryStorage ,PoetryStorage>();
        serviceCollection.AddSingleton<ResultViewModel>();
        serviceCollection.AddSingleton<TodayViewModel>();
        serviceCollection.AddSingleton<ITodayPoetryService , JinrishiciService>();
        serviceCollection.AddSingleton<IAlertService, AlertService>();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}