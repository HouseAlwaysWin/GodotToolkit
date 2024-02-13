using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using GodotToolkit.Services;
using MudBlazor.Services;

namespace GodotToolkit;

public static class Startup
{
    public static IServiceProvider? Services { get; private set; }

    public static void Init()
    {
        var host = Host.CreateDefaultBuilder()
                       .ConfigureServices(WireupServices)
                       .Build();
        Services = host.Services;
    }

    private static void WireupServices(IServiceCollection services)
    {
        services.AddWindowsFormsBlazorWebView();
        services.AddSingleton<IConfigServices, ConfigServices>();
        services.AddTransient<IFolderPicker, FolderPicker>();
        services.AddSingleton<LocalizationService>();
        services.AddMudServices();

        var rootPath = @$"{AppDomain.CurrentDomain.BaseDirectory}Generator\Templates";
        services.AddMvcCore().AddRazorRuntimeCompilation(opts =>
        {
            opts.FileProviders.Add(new PhysicalFileProvider(rootPath)); // This will be the root path
        });
        services.AddRazorTemplating();

#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
    }
}
