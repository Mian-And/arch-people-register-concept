using Client_ConceitoCadastro.Core.Application;
using Client_ConceitoCadastro.Core.Application.Ports;
using Client_ConceitoCadastro.Core.Application.UseCases.GetZipCode;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using System.Windows;

namespace Client_ConceitoCadastro;

public partial class App : Application
{
    public static IHost AppHost { get; } = Host.CreateDefaultBuilder()
        .ConfigureServices((ctx, services) =>
        {
            //// HTTP/Infra
            services.AddHttpClient();

            //// CEP (troque aqui por CorreiosSoapCepLookupService  se quiser)
            services.AddTransient<ICepLookupService, ViaCepLookupService>();
            //services.AddTransient<ICepLookupService, CorreiosSoapCepLookupService>();

            //use cases
            services.AddTransient<GetZipCodeHandler>();

            // View + VM
            services.AddSingleton<MainWindow>();
            services.AddTransient<AddressViewModel>();
        })
        .Build();

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost.StartAsync();

        var main = AppHost.Services.GetRequiredService<MainWindow>();
        // se precisar da VM via DI:
        // main.DataContext = AppHost.Services.GetRequiredService<AddressViewModel>();

        MainWindow = main;
        main.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost.StopAsync();
        AppHost.Dispose();
        base.OnExit(e);
    }
}
