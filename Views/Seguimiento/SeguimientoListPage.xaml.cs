using Exportacion.ViewModels;

namespace Exportacion.Views.Seguimiento;

public partial class SeguimientoListPage : ContentPage
{
    private SeguimientoListPageViewModel _viewMode;
	public  SeguimientoListPage(SeguimientoListPageViewModel vieModel)
	{
		InitializeComponent();
		_viewMode = vieModel;
		this.BindingContext = vieModel;
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewMode.GetSeguimientotListCommand.Execute(null);
	}
}