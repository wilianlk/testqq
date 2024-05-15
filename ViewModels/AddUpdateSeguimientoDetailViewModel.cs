using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Exportacion.Models;
using Exportacion.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exportacion.ViewModels
{
    [QueryProperty(nameof(SeguimientoDetail), "SeguimientoDetail")]
    public partial class AddUpdateSeguimientoDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private SeguimientoModel _seguimientoDetail = new SeguimientoModel();

        private readonly ISeguimientoService _seguimientoService;
        public AddUpdateSeguimientoDetailViewModel(ISeguimientoService seguimientoService)
        {
            _seguimientoService = seguimientoService;
        }

        [RelayCommand]
        public async void AddUpdateSeguimiento()
        {
            int response = -1;
            if (SeguimientoDetail.Id_seguimiento > 0)
            {
                response = await _seguimientoService.UpdateSeguimiento(SeguimientoDetail);
            }
            else
            {
                response = await _seguimientoService.AddSeguimiento(new Models.SeguimientoModel
                {
                    Exportacion = SeguimientoDetail.Exportacion,
                    Fecha = SeguimientoDetail.Fecha,

                });
            }

            if (response > 0)
            {
                await Shell.Current.DisplayAlert("Seguimiento Informacion Guardada", "Registro Guardado", "OK");
                await Shell.Current.GoToAsync("seguimientoList");
                await _seguimientoService.GetSeguimientoList();

            }
            else
            {
                await Shell.Current.DisplayAlert("¡Aviso!", "Algo salio mal al agregar el registro", "OK");
            }
        }


    }
}
