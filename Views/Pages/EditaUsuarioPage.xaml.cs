using Exportacion.Models;

namespace Exportacion.Views.Pages;

public partial class EditaUsuarioPage : ContentPage
{
	UsuarioModel _usuariomodel;
	public EditaUsuarioPage()
	{
		InitializeComponent();
		_usuariomodel = new UsuarioModel();

		this.BindingContext = _usuariomodel;
	}
    private async void btnEntrar_Clicked(object sender, EventArgs e)
    {
		if(string.IsNullOrWhiteSpace(_usuariomodel.Email) &&
			string.IsNullOrWhiteSpace(_usuariomodel.Contrasena))
		{
			await DisplayAlert("Atencion", "", "Cerrar");
            return;
        }

		var cadastro = await App.BancoDados.UsuarioDataTable.GuardarUsuario(_usuariomodel);

		if(cadastro > 0)
		{
			await Navigation.PopAsync();
		}
		
    }

    private async void btnVoltar_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}