using Exportacion.Views.Seguimiento;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;


namespace Exportacion.ViewModels
{
    public partial class SeguimientoListPageViewModel : ObservableObject
    {
        [ICommand]
        public async void AddUpdateSeguimiento()
        {
            await AppShell.Current.GoToAsync(nameof(AddUpdateSeguimientoDetail));
        }

    }
}
