using Exportacion.ViewModels;
using Exportacion.Views;
using Exportacion.Services;

namespace Exportacion
{
    public partial class App : Application
    {
        public static bool IsUserLoggedIn { get; set; } = false;
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            MainPage = IsUserLoggedIn ? new NavigationPage(new Views.MainPage()) : new NavigationPage(new Views.Login.LoginPage());


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
        protected override void OnStart()
        {
        }
        protected override void OnSleep()
        {
        }
        protected override void OnResume()
        {
        }

    }
}
