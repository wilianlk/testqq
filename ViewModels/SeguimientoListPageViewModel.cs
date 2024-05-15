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
                foreach (var seguimiento in seguimientoList)
                {
                    Seguimiento.Add(seguimiento);
                }
                SeguimientoListForSearch.Clear();
                SeguimientoListForSearch.AddRange(seguimientoList);

            }
        }

        [RelayCommand]
        public async void AddUpdateSeguimiento()
        {
            await AppShell.Current.GoToAsync(nameof(AddUpdateSeguimientoDetail));
        }

        [RelayCommand]
        public async void EditSeguimiento(SeguimientoModel seguimientoModel)
        {
            var navParam = new Dictionary<string, object>();
            navParam.Add("SeguimientoDetail", seguimientoModel);
            await AppShell.Current.GoToAsync(nameof(AddUpdateSeguimientoDetail), navParam);
        }

        [RelayCommand]
        public async void DeleteSeguimiento(SeguimientoModel seguimientoModel)
        {
            var delResponse = await _seguimientoService.DeleteSeguimiento(seguimientoModel);
            if (delResponse > 0)
            {
                GetSeguimientotList();
            }
        }

        [RelayCommand]
        public async void DisplayAction(SeguimientoModel seguimientoModel)
        {
            var response = await AppShell.Current.DisplayActionSheet("Seleccionar Opcion", "OK", null, "Editar", "Borrar Registro","Exportar PDF");
            if (response == "Editar")
            {
                var navParam = new Dictionary<string, object>();
                navParam.Add("SeguimientoDetail", seguimientoModel);
                await AppShell.Current.GoToAsync(nameof(AddUpdateSeguimientoDetail), navParam);
            }
            else if (response == "Borrar Registro")
            {
                var delResponse = await _seguimientoService.DeleteSeguimiento(seguimientoModel);
                if (delResponse > 0)
                {
                    GetSeguimientotList();
                }
            }
        }

    }
}
