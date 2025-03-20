using Microsoft.Extensions.Logging;

namespace Expen
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("BeVietnamPro-Medium.ttf", "BeVietnamProMedium");
                    fonts.AddFont("BeVietnamPro-Regular.ttf", "BeVietnamPro");
                    fonts.AddFont("BeVietnamPro-SemiBold.ttf", "BeVietnamProSemiBold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
