using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.Configuration;
using Winder.Repositories;
using Winder.Repositories.Interfaces;
using Controller;

namespace Winder;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
            .AddJsonFile("configdatabase.test.json")
            .Build());

        // Add the repositories
        builder.Services.AddSingleton<IChatMessageRepository, ChatMessageRepository>();
        builder.Services.AddSingleton<ChatMessageController>(sp =>
            new ChatMessageController(sp.GetService<IChatMessageRepository>()));
        // Initialise the toolkit
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();
    //    IConfigurationRoot configuration = new ConfigurationBuilder()
    //.AddJsonFile("configdatabase.json")
    //.Build();

    //    builder.Services.AddSingleton<IConfiguration>(configuration);
    //    builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}