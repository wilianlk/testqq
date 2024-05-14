using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Exportacion.Models;
using Exportacion.Services;
using Exportacion.Views.Seguimiento;
using System.Collections.ObjectModel;


namespace Exportacion.ViewModels
{
    public partial class SeguimientoListPageViewModel : ObservableObject
    {
        public static List<SeguimientoModel> SeguimientoListForSearch { get; private set; } = new List<SeguimientoModel>();
        public ObservableCollection<SeguimientoModel> Seguimiento { get; set; } = new ObservableCollection<SeguimientoModel>();

        private readonly ISeguimientoService _seguimientoService;
        public SeguimientoListPageViewModel(ISeguimientoService seguimientoService)
        {
            _seguimientoService = seguimientoService;
        }

        [RelayCommand]
        public async void GetSeguimientotList()
        {
            Seguimiento.Clear();
            var seguimientoList = await _seguimientoService.GetSeguimientoList();
            if(seguimientoList?.Count > 0) 
            {
                seguimientoList = seguimientoList.OrderBy(f => f.Exportacion).ToList();

            }
        }

        [RelayCommand]
        public async void AddUpdateSeguimiento()
        {
            await AppShell.Current.GoToAsync(nameof(AddUpdateSeguimientoDetail));
        }

    }
}
