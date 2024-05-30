using Exportacion.ViewModels;
using Exportacion.Views;
using Exportacion.Services;

namespace Exportacion
{
    public partial class App : Application
    {
        static SQLiteData _bancoDados;

        public static SQLiteData BancoDados
        {
            get
            {
                if (_bancoDados == null)
                {
                    _bancoDados =
                        new SQLiteData(Path.Combine(Environment.
                        GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Exportacion.db3"));
                }
                return _bancoDados;
            }
        }
        public static UsuarioData usuario { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif __IOS__
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                handler.PlatformView.Background = null;
                handler.PlatformView.FocusVisualMargin = new Microsoft.UI.Xaml.Thickness(0);
#endif
            }); 
        }

    }
}
