using Microsoft.Extensions.Logging;
using GestorComercial_Movil.Handlers; 

namespace GestorComercial_Movil
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
                });

            builder.Services.AddMauiBlazorWebView();

            // 1. Registramos nuestro interceptor de seguridad (Handler)
            builder.Services.AddTransient<TokenAuthenticationHandler>();

            // 2. Configuramos el HttpClient para que pase por el interceptor antes de salir a internet
            builder.Services.AddScoped(sp =>
            {
                var handler = sp.GetRequiredService<TokenAuthenticationHandler>();
                handler.InnerHandler = new HttpClientHandler();

                return new HttpClient(handler)
                {
                    // Asegúrate de cambiar esta URL por la dirección real de tu API en Monster o localhost
                    BaseAddress = new Uri("https://tu-dominio-monster.com/api/")
                };
            });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}