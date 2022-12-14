using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;


namespace Winder;

public static class MauiProgram
{
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


        // Initialise the toolkit
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}