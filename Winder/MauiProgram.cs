using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.Configuration;
using Winder.Repositories;
using Winder.Repositories.Interfaces;
using Controller;

namespace Winder
{
    public static class MauiProgram
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static MauiApp CreateMauiApp()
        {
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
                .AddJsonFile("configdatabase.json")
                .Build());
            // Add the repositories
            builder.Services.AddSingleton<IChatMessageRepository, ChatMessageRepository>();
            builder.Services.AddSingleton<ChatController>(sp => new ChatController(sp.GetService<IChatMessageRepository>()));
            builder.Services.AddSingleton<IInterestsRepository, InterestsRepository>();
            builder.Services.AddSingleton<InterestController>(sp => new InterestController(sp.GetService<IInterestsRepository>()));
            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<UserController>(sp => new UserController(sp.GetService<IUserRepository>()));
            // Initialise the toolkit
            builder.UseMauiApp<App>().UseMauiCommunityToolkit();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();
            ServiceProvider = app.Services;
            return app;
        }
    }

}
