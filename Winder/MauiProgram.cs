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
            builder.Services.AddSingleton<ValidationController>(sp => new ValidationController(sp.GetService<IUserRepository>()));
            builder.Services.AddSingleton<ResetPasswordController>(sp => new ResetPasswordController(sp.GetService<IUserRepository>()));
            builder.Services.AddSingleton<SettingsController>(sp => new SettingsController(sp.GetService<IUserRepository>()));
            builder.Services.AddSingleton<SignInController>(sp => new SignInController(sp.GetService<IUserRepository>()));
            builder.Services.AddSingleton<RegistrationController>(sp => new RegistrationController(sp.GetService<IUserRepository>()));
            builder.Services.AddSingleton<IMatchRepository, MatchRepository>();

            builder.Services.AddSingleton<ILikedRepository, LikedRepository>();
            builder.Services.AddSingleton<LikeDislikeController>(sp => new LikeDislikeController(sp.GetService<ILikedRepository>(), sp.GetService<IUserRepository>(), sp.GetService<IMatchRepository>(), sp.GetService<IPhotosRepository>()));

            builder.Services.AddSingleton<MatchmakingController>(sp => new MatchmakingController(sp.GetService<IUserRepository>(), sp.GetService<ILikedRepository>(),sp.GetService<IPhotosRepository>()));
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
