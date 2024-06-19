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
            string dbPath = DatabaseHelper.GetDatabasePath();
            if (!string.IsNullOrEmpty(dbPath))
            {
                _authService = new AuthService(dbPath);
            }
            else
            {
                DisplayAlert("Error", "No se pudo inicializar el servicio de autenticación.", "OK");
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var username = usernameEntry.Text;
            var password = passwordEntry.Text;

            bool isValid = await _authService.Login(username, password);

            if (isValid)
            {
                App.IsUserLoggedIn = true;
                App.CurrentUser = username;
                // Navegar a la página principal o dashboard
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                // Mostrar un mensaje de error
                await DisplayAlert("Login Failed", "Usuario o Contraseña Invalido", "OK");
            }
        }
    }
}
