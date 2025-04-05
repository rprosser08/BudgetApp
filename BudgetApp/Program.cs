using Avalonia;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BudgetApp.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BudgetApp;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        LoadEnvironmentVariables();
        
        var host = CreateHostBuilder(args).Build();
        EnsureDatabaseCreatedAsnyc(host);
        
        StartAvaloniaApp(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

    private static void LoadEnvironmentVariables()
    {
        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var envFilePath = Path.Combine(projectRoot, ".env");

        if (!File.Exists(envFilePath))
        {
            Console.WriteLine("Environment variables not found");
            return;
        }
        Env.Load(envFilePath);
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
                });
            });

    private static void EnsureDatabaseCreatedAsnyc(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            try
            {
                dbContext.Database.Migrate();
                Console.WriteLine("Migrated");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    private static void StartAvaloniaApp(string[] args)
    {
        var builder = BuildAvaloniaApp();
        builder.StartWithClassicDesktopLifetime(args);
    }
}