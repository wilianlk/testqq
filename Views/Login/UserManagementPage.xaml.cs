using System;
using Microsoft.Maui.Controls;
using Exportacion.Models;
using Exportacion.Services;
using Exportacion.Helpers;

namespace Exportacion.Views.Login
{
    public partial class UserManagementPage : ContentPage
    {
        private AuthService _authService;
        private UsuarioModel _selectedUser;

        public UserManagementPage()
        {
            InitializeComponent();
            _authService = new AuthService(DatabaseHelper.GetDatabasePath());
            LoadUsers();
        }
        private async void LoadUsers()
        {
            usersListView.ItemsSource = await _authService.GetAllUsers();
        }
        private async void OnAddUserClicked(object sender, EventArgs e)
        {
            var username = usernameEntry.Text;
            var password = passwordEntry.Text;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                await _authService.AddUser(username, password);
                LoadUsers();
                usernameEntry.Text = string.Empty;
                passwordEntry.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("Error", "Username and Password are required", "OK");
            }
        }
        private void OnUserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _selectedUser = e.SelectedItem as UsuarioModel;
            if (_selectedUser != null)
            {
                usernameEntry.Text = _selectedUser.Username;
                passwordEntry.Text = _selectedUser.Password;
            }
        }

        private async void OnUpdateUserClicked(object sender, EventArgs e)
        {
            if (_selectedUser != null)
            {
                _selectedUser.Username = usernameEntry.Text;
                _selectedUser.Password = BCrypt.Net.BCrypt.HashPassword(passwordEntry.Text);
                await _authService.UpdateUser(_selectedUser);
                LoadUsers();
                _selectedUser = null;
                usernameEntry.Text = string.Empty;
                passwordEntry.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("Error", "No user selected", "OK");
            }
        }

        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            if (_selectedUser != null)
            {
                await _authService.DeleteUser(_selectedUser.Id);
                LoadUsers();
                _selectedUser = null;
                usernameEntry.Text = string.Empty;
                passwordEntry.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("Error", "No user selected", "OK");
            }
        }
    }
}
