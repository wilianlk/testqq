using Exportacion.ViewModels;

namespace Exportacion.Views.Seguimiento;

public partial class AddUpdateSeguimientoDetail : ContentPage
{
	public AddUpdateSeguimientoDetail(AddUpdateSeguimientoDetailViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel; 
	}
}