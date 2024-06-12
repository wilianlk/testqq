using System;
using Exportacion.Helpers;
using Exportacion.Services;
using Microsoft.Maui.Controls;

namespace Exportacion.Views.Login
{
    public partial class LoginPage : ContentPage
    {
        private AuthService _authService;

        public LoginPage()
        {
            InitializeComponent();
            _authService = new AuthService(DatabaseHelper.GetDatabasePath());
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var username = usernameEntry.Text;
            var password = passwordEntry.Text;

            bool isValid = await _authService.Login(username, password);

            if (isValid)
            {
                App.IsUserLoggedIn = true;
                // Navegar a la página principal o dashboard
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                // Mostrar un mensaje de error
                await DisplayAlert("Login Failed", "Invalid username or password", "OK");
            }
        }
    }
}
