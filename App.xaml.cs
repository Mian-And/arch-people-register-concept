using Client_ConceitoCadastro.Core.Application;
using Client_ConceitoCadastro.Core.Application.Ports;
using Client_ConceitoCadastro.Core.Application.UseCases.GetZipCode;
using Client_ConceitoCadastro.Core.Application.UseCases.SendMessage;
using Client_ConceitoCadastro.Infrastructure.Persistence;
using Client_ConceitoCadastro.Infrastructure.ZipCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Windows;

namespace Client_ConceitoCadastro;

public partial class App : Application
{
    public static IHost AppHost { get; } = Host.CreateDefaultBuilder()
        .ConfigureServices((ctx, services) =>
        {
            // EF/DB
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlite($"Data Source={SqlitePaths.GetDbPath()}")
                .EnableSensitiveDataLogging()
                .LogTo(msg => System.Diagnostics.Debug.WriteLine(msg),
                Microsoft.Extensions.Logging.LogLevel.Information);
                //var dbPath = Path.Combine(
                //    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                //    "Client_ConceitoCadastro", "app.db");
                //Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
                //opt.UseSqlite($"Data Source={dbPath}");
            });

            //// HTTP/Infra
            //services.AddHttpClient();
            services.AddHttpClient<ICepLookupService, ViaCepLookupService>(c => c.Timeout = TimeSpan.FromSeconds(8));
            services.AddHttpClient<ICepLookupService, ViaCepLookupService>(c =>
            {
                c.BaseAddress = new Uri("https://viacep.com.br");
                c.Timeout = TimeSpan.FromSeconds(8);
            });
            // and later: await _http.GetAsync($"/ws/{normalizedCep}/json/", ct);

            //// CEP (troque aqui por CorreiosSoapCepLookupService  se quiser)
            services.AddTransient<ICepLookupService, ViaCepLookupService>();
            //services.AddTransient<ICepLookupService, CorreiosSoapCepLookupService>();
            
            services.AddScoped<IDeliveryRepository, DeliveryRepository>();
            services.AddScoped<CreateDeliveryHandler>();
            //use cases
            services.AddTransient<GetZipCodeHandler>();

            // View + VM
            services.AddTransient<DeliveryViewModel>();
            services.AddSingleton<MainWindow>();
        })
        .Build();

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost.StartAsync();
        using var scope = AppHost.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();            // aplica migrations pendentes no arquivo configurado abaixo

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
