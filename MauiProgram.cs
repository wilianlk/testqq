using CommunityToolkit.Maui;
using Exportacion.Services;
using Exportacion.ViewModels;
using Exportacion.Views;
using Exportacion.Views.Seguimiento;
using Microsoft.Extensions.Logging;

namespace Exportacion
{
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            //Services
            builder.Services.AddSingleton<ISeguimientoService,SeguimientoService>();

            // View Registration
            builder.Services.AddSingleton<SeguimientoListPage>();
            builder.Services.AddTransient<AddUpdateSeguimientoDetail>();

            //view Models
            builder.Services.AddSingleton<SeguimientoListPageViewModel>();
            builder.Services.AddTransient<AddUpdateSeguimientoDetailViewModel>();

            return builder.Build();
        }
    }
}
