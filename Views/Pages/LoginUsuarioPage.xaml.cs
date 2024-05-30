namespace Exportacion.Views.Pages;

public partial class LoginUsuarioPage : ContentPage
{
	public LoginUsuarioPage()
	{
		InitializeComponent();
	}

    private void btnRegistrar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EditaUsuarioPage());
    }

    private async void btnEntrar_Clicked(object sender, EventArgs e)
    {
        string email = txtEmail.Text;
        string senha = txtSenha.Text;

        if(!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(senha))
        {
            var usuario = App.BancoDados.UsuarioDataTable.ObtenerUsuario(email, senha);

            if(usuario != null )
            {
                await DisplayAlert("Atencion", "", "Cerrar");
                return;
            }

            //App.usuario = usuario;
        }
    }
}